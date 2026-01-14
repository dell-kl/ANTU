# 📱 ANTU – Sistema de Gestión Productiva para Balanceado de Cerdos

**ANTU** es un aplicativo móvil para la plataforma **Android**, desarrollado para un **emprendimiento familiar** dedicado a la **fabricación y venta de balanceado para cerdos**.

El sistema permite llevar un control integral de **materia prima, procesos de fabricación, empaquetado, ventas e ingresos/egresos**, reflejando de forma simple pero ordenada la operación real del negocio.

El objetivo principal de ANTU es **centralizar la información**, reducir errores manuales y ofrecer **visibilidad mensual** sobre los costos y ganancias del emprendimiento.

---

## 🎯 Propósito del Sistema

ANTU nace de la necesidad de digitalizar y optimizar la gestión operativa de un pequeño emprendimiento familiar. Antes de su implementación, el control se realizaba de forma manual, lo que generaba:

- Pérdida de información
- Errores en el cálculo de costos
- Dificultad para realizar seguimiento del inventario
- Falta de visibilidad sobre rentabilidad mensual

Con ANTU, el negocio puede:
- Llevar un control preciso del stock de materia prima
- Tener trazabilidad del proceso productivo
- Conocer en tiempo real el inventario de productos terminados
- Generar reportes mensuales de ingresos y egresos

---

## 🧩 Funcionalidades Principales

### 1. Gestión de Materia Prima

- Registro y administración (CRUD) de materias primas.
- Control de **stock en kilogramos** como unidad base.
- Registro de ingresos de materia prima con:
  - Precio variable por ingreso
  - Cantidad adquirida (kg)
- El stock se **incrementa únicamente por ingresos** y se **reduce únicamente por procesos de fabricación**.
- Soporte para imágenes asociadas a cada materia prima.

> **Nota:** El sistema permite que el precio y la cantidad varíen en cada ingreso, manteniendo siempre un stock acumulado actualizado.

---

### 2. Catálogo de Productos

- Registro y administración de los productos finales que se comercializan.
- Cada producto puede definir:
  - Nombre del producto
  - Tipo/categoría
  - Peso estándar del costal (ej. 30kg, 40kg)
  - Precio de venta
- Gestión de imágenes asociadas a cada producto del catálogo.

---

### 3. Proceso de Fabricación

- Registro del proceso de fabricación de productos.
- Definición de las materias primas utilizadas y sus cantidades (kg).
- **Validación automática de stock disponible** antes de iniciar la fabricación.
- **Descuento automático** de materia prima desde bodega.
- Control de estado del proceso (iniciado, finalizado).

Este módulo permite tener **trazabilidad básica** del consumo de insumos durante la producción.

---

### 4. Empaquetado

- Conversión del producto fabricado en **costales listos para la venta**.
- Registro del tipo de costal (peso) y cantidad producida.
- **Incremento del inventario** de producto terminado.
- Cierre del proceso productivo.

El sistema valida que el empaquetado no exceda la cantidad producida en el proceso de fabricación.

---

### 5. Ventas

- Registro de ventas de productos empacados.
- Selección del producto del catálogo.
- Registro de cantidad de costales vendidos y fecha de venta.
- **Descuento automático** del inventario de producto terminado.
- **Generación de ingresos económicos** registrados en el sistema.

---

### 6. Reportes

- Visualización de **ingresos y egresos** por mes.
- Reportes basados en:
  - Consumo de materia prima (egresos)
  - Ingresos por ventas
- Representación mediante **tablas y/o gráficos** para facilitar el análisis.

---

## 🔄 Flujo General del Sistema

```text
┌──────────────────────────────┐
│ Ingreso de Materia Prima     │
│ (+ Stock, Registro de Precio)│
└──────────────┬───────────────┘
               ↓
┌──────────────────────────────┐
│ Fabricación                  │
│ (- Materia Prima, Validación)│
└──────────────┬───────────────┘
               ↓
┌──────────────────────────────┐
│ Empaquetado                  │
│ (Generación de Costales)     │
└──────────────┬───────────────┘
               ↓
┌──────────────────────────────┐
│ Inventario Producto Terminado│
└──────────────┬───────────────┘
               ↓
┌──────────────────────────────┐
│ Ventas                       │
│ (- Inventario, + Ingresos)   │
└──────────────┬───────────────┘
               ↓
┌──────────────────────────────┐
│ Reportes Mensuales           │
│ (Análisis Financiero)        │
└──────────────────────────────┘
```

---

## 🛠️ Tecnologías y Stack Técnico

### Framework y Plataforma
- **.NET 10** (net10.0-android)
- **.NET MAUI** - Framework multiplataforma para aplicaciones móviles
- **C#** - Lenguaje de programación principal
- **XAML** - Definición de interfaces de usuario

### Plataforma Objetivo
- **Android** (API Level 21+)
- Compatible con **Android 15** y versiones anteriores

### Librerías y Paquetes Principales

#### UI y UX
- **Syncfusion.Maui.*** (v31.2.10) - Componentes avanzados de UI
  - DataGrid
  - DataForm
  - ListView
  - Toolbar
  - Expander
- **CommunityToolkit.Maui** (v13.0.0) - Herramientas adicionales de MAUI
- **Mopups** (v1.3.4) - Gestión de ventanas emergentes

#### Arquitectura y Patrones
- **CommunityToolkit.Mvvm** (v8.4.0) - Implementación del patrón MVVM
- **Microsoft.Extensions.DependencyInjection** - Inyección de dependencias

#### Comunicación HTTP
- **Microsoft.Extensions.Http** (v10.0.0)
- **Microsoft.Extensions.Http.Resilience** (v10.0.0) - Políticas de reintentos y resiliencia
- **Newtonsoft.Json** (v13.0.4) - Serialización/deserialización JSON

### API REST Backend
El proyecto consume un API REST desarrollado en ASP.NET Core que gestiona toda la lógica de negocio y persistencia de datos.

🔗 **Repositorio del API:** [ServicioApiBodegaBalanceado](https://github.com/dell-kl/ServicioApiBodegaBalanceado)

---

## 📁 Arquitectura y Estructura del Proyecto

El proyecto sigue el patrón **MVVM (Model-View-ViewModel)** con una arquitectura limpia y separación de responsabilidades:

```
ANTU/
│
├── Models/                          # Modelos de datos y DTOs
│   ├── CatalogoProducto.cs
│   ├── MateriaPrimaProducto.cs
│   ├── Produccion.cs
│   ├── ProductosListos.cs
│   ├── Dto/                         # Data Transfer Objects
│   └── RequestDto/                  # DTOs para requests al API
│
├── ViewModel/                       # ViewModels (lógica de presentación)
│   ├── DashboardViewModel.cs
│   ├── MateriaPrimaViewModel.cs
│   ├── CatalogoProductoFormularioViewModel.cs
│   ├── FabricacionFormularioViewModel.cs
│   ├── ComponentsViewModel/         # ViewModels de componentes reutilizables
│   └── PopupServicesViewModel/      # ViewModels de popups
│
├── Views/                           # Vistas XAML
│   ├── dashboard/                   # Pantalla principal
│   ├── Login/                       # Autenticación (desactivado)
│   ├── Formularios/                 # Formularios de registro/edición
│   ├── Detalles/                    # Vistas de detalle
│   ├── Contabilidad/                # Reportes financieros
│   └── Config/                      # Configuración de la app
│
├── Resources/
│   ├── Components/                  # Componentes reutilizables XAML
│   │   ├── CollectionViewComponents/
│   │   ├── FormularioComponentes/
│   │   ├── PopupComponents/
│   │   └── ControlersComponents/
│   ├── Rest/                        # Capa de comunicación HTTP
│   │   ├── RestManagement.cs       # Cliente HTTP principal
│   │   ├── Endpoints.cs            # Definición de endpoints
│   │   ├── MateriaPrimaRest.cs
│   │   ├── CatalogoProductoRest.cs
│   │   ├── ProduccionRest.cs
│   │   └── RestInterfaces/
│   ├── Messenger/                   # Sistema de mensajería interno
│   ├── Utilidades/                  # Helpers y utilidades
│   ├── ValueConverter/              # Conversores XAML
│   ├── Customizer/                  # Personalizaciones de UI
│   ├── Styles/                      # Estilos globales
│   ├── Images/                      # Recursos de imagen
│   └── Fonts/                       # Fuentes personalizadas
│
├── Platforms/                       # Código específico de plataforma
│   └── Android/
│
├── App.xaml                         # Configuración global de la app
├── AppShell.xaml                    # Shell navigation
├── MauiProgram.cs                   # Punto de entrada y configuración DI
└── README.md                        # Este archivo
```

---

## 🔧 Requisitos Previos

### Software Necesario

1. **Visual Studio 2022** (v17.12 o superior)
   - Workload: ".NET Multi-platform App UI development"
   - Componentes de Android SDK

2. **.NET 10 SDK** (o superior)
   - Verificar con: `dotnet --version`

3. **Android SDK**
   - API Level 21 o superior
   - Android Emulator o dispositivo físico

4. **JDK 17** (para Android build)

### Herramientas Recomendadas

- **Android Studio** (para emulador y debugging avanzado)
- **Postman** o **Swagger** (para probar el API REST)
- **Git** para control de versiones

---

## ⚙️ Configuración del Entorno

### 1. Clonar el Repositorio

```bash
git clone <URL_DEL_REPOSITORIO>
cd ANTU
```

### 2. Configurar el API REST

El aplicativo móvil requiere que el API REST esté ejecutándose. 

**Pasos:**
1. Clonar el repositorio del API: [ServicioApiBodegaBalanceado](https://github.com/dell-kl/ServicioApiBodegaBalanceado)
2. Seguir las instrucciones de configuración del README del API
3. Asegurarse de que el API esté corriendo en la dirección configurada

### 3. Configurar la URL del API en la App

Editar el archivo `MauiProgram.cs` y modificar la URL base del API:

```csharp
var n = builder.Services.AddHttpClient("HttpClientRest", client =>
{
    // Cambiar esta URL según tu configuración
    #if DEBUG
    client.BaseAddress = new Uri("http://192.168.100.253:5055");
    #endif
});
```

**Configuraciones comunes:**
- Emulador Android: `http://10.0.2.2:5055`
- Dispositivo físico: `http://<IP_DE_TU_PC>:5055`
- Producción: `https://tu-api.com`

### 4. Restaurar Dependencias

```bash
dotnet restore
```

### 5. Compilar el Proyecto

```bash
dotnet build
```

---

## 🚀 Ejecución del Proyecto

### Opción 1: Visual Studio (Recomendado)

1. Abrir `ANTU.sln` en Visual Studio 2022
2. Seleccionar el target de Android
3. Elegir un emulador o dispositivo físico
4. Presionar **F5** o hacer clic en "Run"

### Opción 2: CLI de .NET

```bash
# Para Android
dotnet build -t:Run -f net10.0-android
```

### Opción 3: Instalar APK en Dispositivo

```bash
# Generar APK de release
dotnet publish -f net10.0-android -c Release

# El APK se generará en:
# bin/Release/net10.0-android/publish/
```

---

## 🌐 API REST - Endpoints Principales

### Materia Prima

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/RawMaterial/RegistrarMateriaPrima` | Registrar nueva materia prima |
| POST | `/api/RawMaterial/RegistrarImagenes` | Subir imágenes de materia prima |
| GET | `/api/RawMaterial/SolicitarMateriaPrima` | Obtener lista de materias primas |
| GET | `/api/RawMaterial/DetalleMateriaPrima` | Obtener detalle de una materia prima |
| POST | `/api/RawMaterial/AgregarEnStock` | Registrar ingreso de stock |
| PUT | `/api/RawMaterial/EditNameRawMaterial` | Editar nombre de materia prima |
| DELETE | `/api/RawMaterial/DeleteRawMaterial` | Eliminar materia prima |
| GET | `/api/RawMaterial/SolicitarKgMonitoring` | Obtener seguimiento de kg |

### Catálogo de Productos

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/CatalogProduct/RegistrarDataCatalogProduct` | Registrar nuevo producto |
| POST | `/api/CatalogProduct/AgregarDataProductDataCatalogProduct` | Agregar datos al producto |
| POST | `/api/CatalogProduct/RegistrarImagenes` | Subir imágenes del producto |
| GET | `/api/CatalogProduct/SolicitarCatalogProduct` | Obtener lista de productos |
| GET | `/api/CatalogProduct/SolicitarDataCatalogProduct` | Obtener datos del producto |
| PUT | `/api/CatalogProduct/EditarDataCatalogProduct` | Editar datos del producto |
| GET | `/api/CatalogProduct/DetalleDataCatalogProduct` | Obtener detalle del producto |
| DELETE | `/api/CatalogProduct/EliminarImagenes` | Eliminar imágenes del producto |

> **Nota:** Para la documentación completa del API, consultar el repositorio: [ServicioApiBodegaBalanceado](https://github.com/dell-kl/ServicioApiBodegaBalanceado)

---

## 🏗️ Patrones y Prácticas Implementadas

### 1. Patrón MVVM (Model-View-ViewModel)

- **Separación clara** entre lógica de presentación y UI
- Uso de `ObservableObject` y `ObservableProperty` del CommunityToolkit.Mvvm
- Data binding bidireccional entre View y ViewModel
- Commands para manejo de eventos de UI

**Ejemplo:**
```csharp
public partial class MateriaPrimaViewModel : ObservableObject
{
    [ObservableProperty]
    private string nombreProducto;
    
    [RelayCommand]
    private async Task GuardarMateriaPrima()
    {
        // Lógica de guardado
    }
}
```

### 2. Inyección de Dependencias (DI)

Configuración centralizada en `MauiProgram.cs`:

```csharp
// Servicios
builder.Services.AddTransient<IRestManagement, RestManagement>();

// ViewModels
builder.Services.AddTransient<MateriaPrimaViewModel>();

// Views
builder.Services.AddTransient<MateriaPrima>();
```

**Beneficios:**
- Facilita el testing unitario
- Reduce acoplamiento entre componentes
- Mejora la mantenibilidad del código

### 3. Resilience y Retry Policies

Implementación de políticas de reintento para llamadas HTTP:

```csharp
n.AddStandardResilienceHandler().Configure(configure =>
{
    // Número de intentos permitidos para lograr la conexión
    configure.Retry.MaxRetryAttempts = 4;
    
    // Timeout de 25 segundos por request
    configure.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(25);
});
```

**Ventajas:**
- Manejo robusto de errores de red
- Mejora la experiencia del usuario en conexiones inestables
- Evita fallos por timeouts temporales

### 4. Navegación con Shell

Uso de `AppShell.xaml` para navegación declarativa:

```csharp
Routing.RegisterRoute("MateriaPrimaDetalle", typeof(MateriaPrimaDetalle));
```

### 5. Componentes Reutilizables

- Separación de componentes UI en `Resources/Components`
- ViewModels específicos para componentes complejos
- Popups con lógica encapsulada

### 6. Manejo de Imágenes

- Conversión a Base64 para envío al API
- Value Converters para renderizado optimizado
- Carga diferida (lazy loading) de imágenes

---

## 📊 Consideraciones Técnicas

### Performance

- **Virtualización** en listas largas mediante `CollectionView`
- **Carga asíncrona** de datos desde el API
- **Caché local** de imágenes cuando es posible

### Manejo de Errores

- Try-catch en operaciones críticas
- Mensajes de error amigables al usuario
- Logging para debugging (solo en DEBUG mode)

### Validación de Datos

- Validación en el cliente antes de enviar al API
- Validación de stock disponible antes de procesos
- Validación de formularios con feedback visual

---

## 🧪 Testing (Futuro)

Actualmente el proyecto no cuenta con tests automatizados, pero se recomienda implementar:

- **Unit Tests** para ViewModels
- **Integration Tests** para servicios REST
- **UI Tests** para flujos críticos (fabricación, ventas)

Framework recomendado: **xUnit** o **NUnit** con **Moq** para mocking.

---

## 📝 Notas Importantes

### Configuración de IP para Desarrollo

Al trabajar con un emulador o dispositivo físico, asegúrate de:

1. **Emulador Android:** Usar `10.0.2.2` para referenciar `localhost` de tu máquina
2. **Dispositivo físico:** Usar la IP local de tu máquina (verificar con `ipconfig` o `ifconfig`)
3. **Firewall:** Asegurarse de que el puerto del API esté accesible

### Licencia de Syncfusion

El proyecto incluye una licencia de Syncfusion configurada en `MauiProgram.cs`. 

⚠️ **Importante:** Esta licencia es para desarrollo/evaluación. Para uso en producción, adquirir una licencia comercial en [syncfusion.com](https://www.syncfusion.com/)

### Sistema de Autenticación

El sistema de login está implementado pero **actualmente desactivado**. La navegación va directamente al Dashboard. Para activarlo en el futuro, modificar el flujo en `App.xaml.cs`.

---

## 🤝 Contribución

Este es un proyecto personal para un emprendimiento familiar. Si deseas contribuir:

1. Fork el repositorio
2. Crea una rama con tu feature: `git checkout -b feature/nueva-funcionalidad`
3. Commit tus cambios: `git commit -m 'Agregar nueva funcionalidad'`
4. Push a la rama: `git push origin feature/nueva-funcionalidad`
5. Abre un Pull Request

---

## 📄 Licencia

Este proyecto es de uso privado para el emprendimiento familiar. No se permite su redistribución sin autorización.

---

## 📧 Contacto

Para consultas sobre el proyecto:
- **Repositorio API:** [ServicioApiBodegaBalanceado](https://github.com/dell-kl/ServicioApiBodegaBalanceado)
- **GitHub:** [@dell-kl](https://github.com/dell-kl)

---

## 🔮 Roadmap Futuro

- [ ] Implementar sistema de autenticación completo
- [ ] Añadir reportes gráficos avanzados
- [ ] Exportación de reportes a PDF/Excel
- [ ] Modo offline con sincronización
- [ ] Notificaciones push para alertas de stock bajo
- [ ] Backup automático de datos
- [ ] Implementar testing automatizado

---

**Desarrollado con ❤️ para optimizar la gestión del emprendimiento familiar**
# SISTEMA ANTU GESTION DE INVENTARIO Y VENTA PRODUCTOS ALIMENTOS DE ANIMALES
