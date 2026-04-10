namespace Data.Rest
{
    public static class Endpoints
    {
        public static string[] ENDPOINTS = {
            "/api/RawMaterial/RegistrarMateriaPrima",
            "/api/RawMaterial/RegistrarImagenes",
            "/api/RawMaterial/SolicitarMateriaPrima",
            "/api/RawMaterial/DetalleMateriaPrima",
            "/api/RawMaterial/AgregarEnStock",
            "/api/RawMaterial/EditNameRawMaterial",
            "api/RawMaterial/ViewImage", // este ruta debe estar asi sin diagonal en el principio
            "/api/RawMaterial/DeleteRawMaterial",
            "/api/RawMaterial/SolicitarKgMonitoring"
        };

        public static string[] VISOR_IMG = { 
            "visor_imagenes"
        };

        public static string[] ENDPOINTS_CATALOGPRODUCT = {
            "/api/CatalogProduct/RegistrarDataCatalogProduct",
            "/api/CatalogProduct/AgregarDataProductDataCatalogProduct",
            "/api/CatalogProduct/RegistrarImagenes",
            "/api/CatalogProduct/SolicitarCatalogProduct",
            "/api/CatalogProduct/SolicitarDataCatalogProduct",
            "/api/CatalogProduct/EditarDataCatalogProduct",
            "/api/CatalogProduct/DetalleDataCatalogProduct", // este ruta debe estar asi sin diagonal en el principio
            "/api/CatalogProduct/EliminarImagenes"
        };

        public static string[] ENDPOINTS_FABRICACION =
        {
            "/api/Manufacture/ObtenerDatosProduccion",
            "/api/Manufacture/GenerarProduccion",
            "/api/Manufacture/EditarEstadoProduccion"
        };

        public static string[] ENDPOINTS_FABRICADO =
        {
            "/api/ReadyProductsManufactured/GetProductionReady",
            "/api/ReadyProductsManufactured/MostrarOpcionesDataCatalogProduct",
            "/api/ReadyProductsManufactured/IngresarProductosABodega",
            "/api/ReadyProductsManufactured/ObtenerInformacionProductosBodega",
            "/api/ReadyProductsManufactured/ActualizarEstadoFabricado"
        };

        public static class MateriaPrimaRutas
        {
            public const string RegistrarMateriaPrima = "/api/RawMaterial/RegistrarMateriaPrima";
            public const string RegistrarImagenes = "/api/RawMaterial/RegistrarImagenes";
            public const string SolicitarMateriaPrima = "/api/RawMaterial/SolicitarMateriaPrima";
            public const string DetalleMateriaPrima = "/api/RawMaterial/DetalleMateriaPrima";
            public const string AgregarEnStock = "/api/RawMaterial/AgregarEnStock";
            public const string EditarNombreMateriaPrima = "/api/RawMaterial/EditNameRawMaterial";
            public const string VerImagen = "api/RawMaterial/ViewImage"; // este ruta debe estar asi sin diagonal en el principio
            public const string EliminarImagenesMateriaPrima = "/api/RawMaterial/DeleteImagesRawMaterial";
            public const string SolicitarKgMonitoring = "/api/RawMaterial/SolicitarKgMonitoring";
        }
    }
}
