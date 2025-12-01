DROP DATABASE IF EXISTS VENTAS;
CREATE DATABASE VENTAS;
USE VENTAS;

-- TABLAS
	create table usuarios(
		IdUsuario int primary key auto_increment,
		
		nombre varchar(50) not null,
		apellidos varchar(75) not null,
		usuario varchar(20) not null,
		rfc char(13) null,
		curp char(18) not null,
		email1 varchar(75) null,
		direccion varchar(100) null,
		telefono1 char(10) not null comment 'Este es el chido',
		telefono2 char(10) null,
		tipoSangre enum('A+', 'A-', 'O+', 'O-', 'B+', 'B-', 'AB+', 'AB-', 'DESCONOCIDO') not null,
		NSS varchar(15) null,
		
		contrasenhaHash varchar(75) not null,
		tipoUsuario enum('Empleado','Administrador') not null default 'Empleado'
	);

	CREATE TABLE categorias(
		IdCategoria INT PRIMARY KEY AUTO_INCREMENT,
		nombreCategoria VARCHAR(50) NOT NULL,
		descripcion VARCHAR(255) NULL
	);

	CREATE TABLE productos(
		IdProducto INT PRIMARY KEY AUTO_INCREMENT,
		nombreProducto VARCHAR(75) NOT NULL,
		descripcion VARCHAR(255) NULL, 
		precio DECIMAL(10,2) NOT NULL,
		existencias INT NOT NULL,
		IdCategoria INT NOT NULL,
		fotoProducto LONGBLOB NULL,
		descontinuado BOOLEAN DEFAULT FALSE,
		
		CONSTRAINT Check_Precio CHECK (precio >= 0),
		CONSTRAINT Check_Stock CHECK (existencias >= 0),
		FOREIGN KEY (IdCategoria) REFERENCES categorias(IdCategoria)
	);

	CREATE TABLE ordenes(
		IdOrden INT PRIMARY KEY AUTO_INCREMENT,
		IdUsuario INT NOT NULL, -- Quién vendió
		FechaOrden DATETIME DEFAULT CURRENT_TIMESTAMP,
		Estatus ENUM('Pagada', 'Pendiente', 'Cancelada') DEFAULT 'Pagada',
		FOREIGN KEY (IdUsuario) REFERENCES usuarios(IdUsuario)
	);

	CREATE TABLE detallesOrden(
		IdDetallesOrden INT PRIMARY KEY AUTO_INCREMENT,
		IdOrden INT NOT NULL,
		IdProducto INT NOT NULL,
		Cantidad INT NOT NULL,
		PrecioUnitario DECIMAL(10,2) NOT NULL, -- Precio al momento de la venta
		Importe DECIMAL(10,2) GENERATED ALWAYS AS (Cantidad * PrecioUnitario) STORED,
		
		FOREIGN KEY (IdOrden) REFERENCES ordenes(IdOrden),
		FOREIGN KEY (IdProducto) REFERENCES productos(IdProducto)
	);

	CREATE TABLE auditoriaProductos(
		IdAuditoria INT PRIMARY KEY AUTO_INCREMENT,
		FechaHora DATETIME DEFAULT CURRENT_TIMESTAMP,
		UsuarioDB VARCHAR(50) NOT NULL,
		TipoCambio VARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
		IdProducto INT NOT NULL,
		NombreProducto VARCHAR(75) NOT NULL,
		ValorAnterior VARCHAR(255) NULL, 
		ValorNuevo VARCHAR(255) NULL
	);


-- TRIGGERS DE AUDITORÍA
DELIMITER $$
	CREATE TRIGGER tr_Producto_Insert AFTER INSERT ON productos
	FOR EACH ROW
	BEGIN
		DECLARE v_user VARCHAR(50);
		SET v_user = IFNULL(@UsuarioActual, CURRENT_USER());
		INSERT INTO auditoriaProductos(UsuarioDB, TipoCambio, IdProducto, NombreProducto, ValorAnterior, ValorNuevo)
		VALUES (v_user, 'INSERT', NEW.IdProducto, NEW.nombreProducto, 'N/A', CONCAT('Stock: ', NEW.existencias, ', Precio: ', NEW.precio));
	END $$

	CREATE TRIGGER tr_Producto_Update AFTER UPDATE ON productos
	FOR EACH ROW
	BEGIN
		DECLARE v_user VARCHAR(50);
		SET v_user = IFNULL(@UsuarioActual, CURRENT_USER());
		-- Solo audita si cambia algo relevante
		INSERT INTO auditoriaProductos(UsuarioDB, TipoCambio, IdProducto, NombreProducto, ValorAnterior, ValorNuevo)
		VALUES (v_user, 'UPDATE', NEW.IdProducto, NEW.nombreProducto, 
				CONCAT('Stock: ', OLD.existencias, ', Precio: ', OLD.precio), 
				CONCAT('Stock: ', NEW.existencias, ', Precio: ', NEW.precio));
	END $$

	CREATE TRIGGER tr_Producto_Delete AFTER DELETE ON productos
	FOR EACH ROW
	BEGIN
		DECLARE v_user VARCHAR(50);
		SET v_user = IFNULL(@UsuarioActual, CURRENT_USER());
		INSERT INTO auditoriaProductos(UsuarioDB, TipoCambio, IdProducto, NombreProducto, ValorAnterior, ValorNuevo)
		VALUES (v_user, 'DELETE', OLD.IdProducto, OLD.nombreProducto, 
				CONCAT('Stock: ', OLD.existencias, ', Precio: ', OLD.precio), 'ELIMINADO');
	END $$


-- FUNCIONES AUXILIARES
	DROP FUNCTION IF EXISTS fn_ObtenerStock $$
	CREATE FUNCTION fn_ObtenerStock(p_Id INT) RETURNS INT
	DETERMINISTIC
	BEGIN
		DECLARE v_stock INT;
		SELECT existencias INTO v_stock FROM productos WHERE IdProducto = p_Id;
		RETURN IFNULL(v_stock, 0);
	END $$



-- STORED PROCEDURES: OPERATIVOS (Login, CRUD)

-- Login
DROP PROCEDURE IF EXISTS sp_ValidarLogin $$
CREATE PROCEDURE sp_ValidarLogin(IN p_User VARCHAR(20), IN p_Pass VARCHAR(75))
BEGIN
    SELECT IdUsuario, nombre, apellidos, usuario, tipoUsuario 
    FROM usuarios 
    WHERE usuario = p_User AND contrasenhaHash = p_Pass;
END $$

-- CRUDs

	-- PRODUCTOS
		-- Insertar Producto
		DROP PROCEDURE IF EXISTS sp_InsertarProducto $$
		CREATE PROCEDURE sp_InsertarProducto(
			IN p_Nombre VARCHAR(75), IN p_Desc VARCHAR(255), IN p_IdCategoria INT,
			IN p_Precio DECIMAL(10,2), IN p_Stock INT, IN p_Foto LONGBLOB, IN p_UsuarioApp VARCHAR(50)
		)
		BEGIN
			SET @UsuarioActual = p_UsuarioApp;
			INSERT INTO productos(nombreProducto, descripcion, IdCategoria, precio, existencias, fotoProducto)
			VALUES (p_Nombre, p_Desc, p_IdCategoria, p_Precio, p_Stock, p_Foto);
		END $$

		-- Actualizar Producto
		DROP PROCEDURE IF EXISTS sp_ActualizarProductoCompleto $$
		CREATE PROCEDURE sp_ActualizarProductoCompleto(
			IN p_IdProducto INT,
			IN p_Nombre VARCHAR(75),
			IN p_Desc VARCHAR(255),
			IN p_IdCategoria INT,
			IN p_Precio DECIMAL(10,2),
			IN p_Stock INT,
			IN p_Foto LONGBLOB,
			IN p_UsuarioApp VARCHAR(50)
		)
		BEGIN
			SET @UsuarioActual = p_UsuarioApp; -- Para la auditoría
			
			UPDATE productos
			SET 
				nombreProducto = p_Nombre,
				descripcion = p_Desc,
				IdCategoria = p_IdCategoria,
				precio = p_Precio,
				existencias = p_Stock,
				fotoProducto = p_Foto 
			WHERE IdProducto = p_IdProducto;
		END $$
        
        DROP PROCEDURE IF EXISTS sp_ObtenerTodosLosProductos $$
		CREATE PROCEDURE sp_ObtenerTodosLosProductos(
			IN p_MostrarDescontinuados BOOLEAN
		)
		BEGIN
			SELECT 
				p.IdProducto,
				CASE 
					WHEN p.descontinuado = 1 THEN CONCAT('DESCONTINUADO - ', c.nombreCategoria, ' - ', p.nombreProducto)
					ELSE CONCAT(c.nombreCategoria, ' - ', p.nombreProducto)
				END AS NombreCompletoDisplay
			FROM productos p
			INNER JOIN categorias c ON p.IdCategoria = c.IdCategoria
			WHERE (p.descontinuado = 0 OR p_MostrarDescontinuados = 1)
			ORDER BY p.descontinuado ASC, c.nombreCategoria ASC, p.nombreProducto ASC;
		END $$

		DROP PROCEDURE IF EXISTS sp_ObtenerProductoPorId $$
		CREATE PROCEDURE sp_ObtenerProductoPorId(IN p_Id INT)
		BEGIN
			SELECT * FROM productos WHERE IdProducto = p_Id;
		END $$

		DROP PROCEDURE IF EXISTS sp_CambiarEstadoProducto $$
		CREATE PROCEDURE sp_CambiarEstadoProducto(
			IN p_IdProducto INT,
			IN p_Estado BOOLEAN,
			IN p_UsuarioApp VARCHAR(50)
		)
		BEGIN
			SET @UsuarioActual = p_UsuarioApp;
			UPDATE productos SET descontinuado = p_Estado WHERE IdProducto = p_IdProducto;
		END $$
        
        DROP PROCEDURE IF EXISTS sp_EliminarProductoFisico $$
		CREATE PROCEDURE sp_EliminarProductoFisico(
			IN p_IdProducto INT,
			IN p_UsuarioApp VARCHAR(50)
		)
		BEGIN
			-- Para que el Trigger de Auditoría sepa quién fue
			SET @UsuarioActual = p_UsuarioApp;

			DELETE FROM detallesOrden WHERE IdProducto = p_IdProducto;
			DELETE FROM productos WHERE IdProducto = p_IdProducto;
		END $$
        
	-- USUARIOS
        DROP PROCEDURE IF EXISTS sp_RegistrarUsuarioCompleto $$
		CREATE PROCEDURE sp_RegistrarUsuarioCompleto(
			IN p_Nombre VARCHAR(50),
			IN p_Apellidos VARCHAR(75),
			IN p_Usuario VARCHAR(20),
			IN p_PassHash VARCHAR(75),
			IN p_Rfc VARCHAR(13),
			IN p_Curp VARCHAR(18),
			IN p_Email1 VARCHAR(75),
			IN p_NSS VARCHAR(15),
			IN p_Direccion VARCHAR(100),
			IN p_Tel1 CHAR(10),
			IN p_Tel2 CHAR(10),
			IN p_TipoSangre VARCHAR(15),
			IN p_TipoUsuario VARCHAR(20)
		)
		BEGIN
			INSERT INTO usuarios(
				nombre, apellidos, usuario, contrasenhaHash, 
				rfc, curp, email1, NSS, direccion, 
				telefono1, telefono2, tipoSangre, tipoUsuario
			) VALUES (
				p_Nombre, p_Apellidos, p_Usuario, p_PassHash,
				p_Rfc, p_Curp, p_Email1, p_NSS, p_Direccion,
				p_Tel1, p_Tel2, p_TipoSangre, p_TipoUsuario
			);
		END $$
  
		DROP PROCEDURE IF EXISTS sp_ActualizarProductoCompleto $$
		CREATE PROCEDURE sp_ActualizarProductoCompleto(
			IN p_IdProducto INT,
			IN p_Nombre VARCHAR(75),
			IN p_Desc VARCHAR(255),
			IN p_IdCategoria INT,
			IN p_Precio DECIMAL(10,2),
			IN p_Stock INT,
			IN p_Foto LONGBLOB,
			IN p_UsuarioApp VARCHAR(50)
		)
		BEGIN
			SET @UsuarioActual = p_UsuarioApp; 
			
			UPDATE productos
			SET 
				nombreProducto = p_Nombre,
				descripcion = p_Desc,
				IdCategoria = p_IdCategoria,
				precio = p_Precio,
				existencias = p_Stock,
				fotoProducto = p_Foto
			WHERE IdProducto = p_IdProducto;
		END $$
        
        DROP PROCEDURE IF EXISTS sp_ObtenerUsuariosResumen $$
		CREATE PROCEDURE sp_ObtenerUsuariosResumen()
		BEGIN
			SELECT 
				IdUsuario,
				CONCAT(nombre, ' ', apellidos) AS NombreCompleto,
				tipoUsuario,
				curp
			FROM usuarios
			ORDER BY tipoUsuario ASC, nombre ASC;
		END $$
        
        DROP PROCEDURE IF EXISTS sp_ObtenerUsuarioPorId $$
		CREATE PROCEDURE sp_ObtenerUsuarioPorId(IN p_Id INT)
		BEGIN
			SELECT * FROM usuarios WHERE IdUsuario = p_Id;
		END $$
        
		DROP PROCEDURE IF EXISTS sp_ActualizarUsuarioCompleto $$
		CREATE PROCEDURE sp_ActualizarUsuarioCompleto(
			IN p_IdUsuario INT,
			IN p_Nombre VARCHAR(50),
			IN p_Apellidos VARCHAR(75),
			IN p_Usuario VARCHAR(20),
			IN p_PassHash VARCHAR(75),
			IN p_Rfc VARCHAR(13),
			IN p_Curp VARCHAR(18),
			IN p_Email1 VARCHAR(75),
			IN p_NSS VARCHAR(15),
			IN p_Direccion VARCHAR(100),
			IN p_Tel1 CHAR(10),
			IN p_Tel2 CHAR(10),
			IN p_TipoSangre VARCHAR(15),
			IN p_TipoUsuario VARCHAR(20)
		)
		BEGIN
			UPDATE usuarios
			SET 
				nombre = p_Nombre,
				apellidos = p_Apellidos,
				usuario = p_Usuario,
				rfc = p_Rfc,
				curp = p_Curp,
				email1 = p_Email1,
				NSS = p_NSS,
				direccion = p_Direccion,
				telefono1 = p_Tel1,
				telefono2 = p_Tel2,
				tipoSangre = p_TipoSangre,
				tipoUsuario = p_TipoUsuario
			WHERE IdUsuario = p_IdUsuario;
            
			IF p_PassHash IS NOT NULL AND p_PassHash <> '' THEN
				UPDATE usuarios SET contrasenhaHash = p_PassHash WHERE IdUsuario = p_IdUsuario;
			END IF;
		END $$


	-- CATEGORIAS
		DROP PROCEDURE IF EXISTS sp_RegistrarCategoria $$
		CREATE PROCEDURE sp_RegistrarCategoria(
			IN p_Nombre VARCHAR(50),
			IN p_Descripcion VARCHAR(255)
		)
		BEGIN
			INSERT INTO categorias(nombreCategoria, descripcion)
			VALUES (p_Nombre, p_Descripcion);
		END $$


-- STORED PROCEDURES - VENTAS (CARRITO)
	-- Crear ticket vacío
	DROP PROCEDURE IF EXISTS sp_CrearOrden $$
	CREATE PROCEDURE sp_CrearOrden(
		IN p_IdUsuario INT,
		OUT p_IdOrdenGenerada INT
	)
	BEGIN
		INSERT INTO ordenes(IdUsuario, Estatus, FechaOrden) VALUES (p_IdUsuario, 'Pagada', NOW());
		SET p_IdOrdenGenerada = LAST_INSERT_ID();
	END $$

	-- Agregar detalle (producto por producto del carrito)
	DROP PROCEDURE IF EXISTS sp_AgregarDetalleOrden $$
	CREATE PROCEDURE sp_AgregarDetalleOrden(
		IN p_IdOrden INT,
		IN p_IdProducto INT,
		IN p_Cantidad INT,
		OUT p_Mensaje VARCHAR(100),
		OUT p_Exito BOOLEAN
	)
	proc_label: BEGIN
		DECLARE v_stock INT;
		DECLARE v_precio DECIMAL(10,2);
		
		SET v_stock = fn_ObtenerStock(p_IdProducto);
		
		IF v_stock < p_Cantidad THEN
			SET p_Mensaje = CONCAT('Stock insuficiente para producto ID: ', p_IdProducto);
			SET p_Exito = FALSE;
			LEAVE proc_label; -- Se sale sin hacer nada
		END IF;

		SELECT precio INTO v_precio FROM productos WHERE IdProducto = p_IdProducto;

		INSERT INTO detallesOrden(IdOrden, IdProducto, Cantidad, PrecioUnitario)
		VALUES (p_IdOrden, p_IdProducto, p_Cantidad, v_precio);

		UPDATE productos SET existencias = existencias - p_Cantidad WHERE IdProducto = p_IdProducto;

		SET p_Mensaje = 'OK';
		SET p_Exito = TRUE;
	END $$


-- STORED PROCEDURES: REPORTES 
	-- Venta por Producto (Rango fechas)
	DROP PROCEDURE IF EXISTS sp_ReporteVentaPorProducto $$
	CREATE PROCEDURE sp_ReporteVentaPorProducto(IN p_FechaInicio DATE, IN p_FechaFin DATE)
	BEGIN
		SELECT 
			p.IdProducto AS Clave,
			p.nombreProducto AS Nombre,
			SUM(d.Cantidad) AS Unidades,
			SUM(d.Importe) AS Monto
		FROM detallesOrden d
		JOIN productos p ON d.IdProducto = p.IdProducto
		JOIN ordenes o ON d.IdOrden = o.IdOrden
		WHERE o.Estatus = 'Pagada' AND DATE(o.FechaOrden) BETWEEN p_FechaInicio AND p_FechaFin
		GROUP BY p.IdProducto, p.nombreProducto
		ORDER BY Monto DESC;
	END $$

	DROP PROCEDURE IF EXISTS sp_ObtenerListaProductosSimple $$
	CREATE PROCEDURE sp_ObtenerListaProductosSimple()
	BEGIN
		SELECT IdProducto, nombreProducto 
		FROM productos 
		WHERE descontinuado = 0 
		ORDER BY nombreProducto;
	END $$

	-- TIDs como un string "1,5,8,10" y usaremos FIND_IN_SET
	DROP PROCEDURE IF EXISTS sp_ReporteComparativoFiltrado $$
	CREATE PROCEDURE sp_ReporteComparativoFiltrado(
		IN p_Mes1 INT, IN p_Anio1 INT,
		IN p_Mes2 INT, IN p_Anio2 INT,
		IN p_ListaIDs TEXT 
	)
	BEGIN
		SELECT 
			p.IdProducto AS Id,
			p.nombreProducto AS Producto,
			p.precio AS Precio,
			
			-- Columna Dinámica Mes 1
			IFNULL(SUM(CASE 
				WHEN MONTH(o.FechaOrden) = p_Mes1 AND YEAR(o.FechaOrden) = p_Anio1 
				THEN d.Importe ELSE 0 END), 0) AS VentasMes1,

			-- Columna Dinámica Mes 2
			IFNULL(SUM(CASE 
				WHEN MONTH(o.FechaOrden) = p_Mes2 AND YEAR(o.FechaOrden) = p_Anio2 
				THEN d.Importe ELSE 0 END), 0) AS VentasMes2

		FROM productos p
		LEFT JOIN detallesOrden d ON p.IdProducto = d.IdProducto
		LEFT JOIN ordenes o ON d.IdOrden = o.IdOrden AND o.Estatus = 'Pagada'
		
		-- TRUCO: FIND_IN_SET busca el ID en la lista separada por comas
		WHERE FIND_IN_SET(p.IdProducto, p_ListaIDs) > 0
		
		GROUP BY p.IdProducto, p.nombreProducto, p.precio
		ORDER BY VentasMes1 DESC, VentasMes2 DESC;
	END $$
    
    -- Auditorias
    DROP PROCEDURE IF EXISTS sp_ObtenerAuditoria $$
	CREATE PROCEDURE sp_ObtenerAuditoria()
	BEGIN
		SELECT 
			IdAuditoria,
			FechaHora,
			UsuarioDB,    
			TipoCambio, 
			NombreProducto,
			ValorAnterior,
			ValorNuevo
		FROM auditoriaProductos
		ORDER BY FechaHora DESC; 
	END $$

DELIMITER ;

-- DATOS DE PRUEBA
-- Categorias
INSERT INTO categorias (nombreCategoria, descripcion) VALUES 
('Pan Dulce', 'Conchas, donas, orejas y pan tradicional dulce'),
('Pan Salado', 'Bolillo, telera, baguette y pan de mesa'),
('Pastelería', 'Pasteles completos y rebanadas'),
('Bebidas', 'Café, leche, chocolate y refrescos'),
('Repostería Fina', 'Galletas decoradas y tartas especiales');

-- Admin
INSERT INTO usuarios (nombre, apellidos, usuario, contrasenhaHash, rfc, curp, email1, telefono1, tipoSangre, tipoUsuario, NSS)
VALUES ('Alfredo', 'Garcia Olmedo', 'root', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 
'XAXX010101000', 'XAXX010101HDFRXX00', 'admin@panaderia.com', '4451234567', 'O+', 'Administrador', '12345678901');

-- Usuario Empleado
INSERT INTO usuarios (nombre, apellidos, usuario, contrasenhaHash, rfc, curp, email1, telefono1, tipoSangre, tipoUsuario, NSS)
VALUES ('Maria', 'López', 'caja', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 
'XAXX020202000', 'XAXX020202MDFRXX00', 'maria@panaderia.com', '4459876543', 'A+', 'Empleado', '98765432109');

-- Insertar productos
-- Pan Dulce
CALL sp_InsertarProducto('Concha Vainilla', 'Clásica concha con cubierta de vainilla', 1, 15.00, 100, NULL, 'Pruebas');
CALL sp_InsertarProducto('Concha Chocolate', 'Clásica concha con cubierta de chocolate', 1, 15.00, 80, NULL, 'Pruebas');
CALL sp_InsertarProducto('Dona Glaseada', 'Dona frita cubierta de azúcar glass', 1, 12.50, 60, NULL, 'Pruebas');
CALL sp_InsertarProducto('Oreja', 'Hojaldre crujiente con azúcar', 1, 14.00, 50, NULL, 'Pruebas');

-- Pan Salado
CALL sp_InsertarProducto('Bolillo', 'Bolillo tradicional crujiente', 2, 8.00, 200, NULL, 'Pruebas');
CALL sp_InsertarProducto('Telera', 'Pan suave ideal para tortas', 2, 9.00, 150, NULL, 'Pruebas');
CALL sp_InsertarProducto('Baguette', 'Barra de pan estilo francés', 2, 25.00, 30, NULL, 'Pruebas');

-- Pastelería
CALL sp_InsertarProducto('Pastel 3 Leches', 'Pastel entero decorado con frutas', 3, 350.00, 5, NULL, 'Pruebas');
CALL sp_InsertarProducto('Rebanada Cheesecake', 'Pay de queso con zarzamora', 3, 45.00, 20, NULL, 'Pruebas');

-- Bebidas
CALL sp_InsertarProducto('Café Americano', 'Vaso 12oz', 4, 25.00, 1000, NULL, 'Pruebas');
CALL sp_InsertarProducto('Chocolate Caliente', 'Estilo oaxaqueño', 4, 30.00, 1000, NULL, 'Pruebas');

-- Ventas
CALL sp_CrearOrden(2, @IdOrden1); 
CALL sp_AgregarDetalleOrden(@IdOrden1, 1, 5, @Msg, @Exito); 
CALL sp_AgregarDetalleOrden(@IdOrden1, 5, 5, @Msg, @Exito); 

CALL sp_CrearOrden(1, @IdOrden2);
CALL sp_AgregarDetalleOrden(@IdOrden2, 8, 1, @Msg, @Exito); 

CALL sp_CrearOrden(2, @IdOrden3);
CALL sp_AgregarDetalleOrden(@IdOrden3, 10, 1, @Msg, @Exito); 
CALL sp_AgregarDetalleOrden(@IdOrden3, 3, 2, @Msg, @Exito);  
CALL sp_AgregarDetalleOrden(@IdOrden3, 9, 1, @Msg, @Exito);  


select * from productos;

select * from auditoriaProductos;