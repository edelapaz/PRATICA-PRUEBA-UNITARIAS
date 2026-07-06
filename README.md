# Practica de Pruebas Unitarias en .NET

Repositorio de ejemplo orientado a pruebas unitarias con xUnit y Moq sobre un controlador ASP.NET Core, siguiendo un enfoque simple de Clean Architecture.

## Objetivo

Este proyecto demuestra como probar un controlador de usuarios cubriendo:

- Casos exitosos y fallidos.
- Verificacion de llamadas con `Verify` y `Times`.
- Verificacion de parametros con `It.Is`.
- Conteo de ejecuciones con `Callback`.
- Flujo completo de multiples operaciones en una sola prueba.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- xUnit
- Moq

## Estructura del proyecto

```text
PRATICA-PRUEBA-UNITARIAS/
|-- PruebasUnitarias.Api/
|   |-- Application/
|   |   `-- IUserService.cs
|   |-- Domain/
|   |   `-- User.cs
|   |-- Infrastructure/
|   |   `-- InMemoryUserService.cs
|   |-- Controllers/
|   |   `-- UsersController.cs
|   `-- Program.cs
|-- PruebasUnitarias.Tests/
|   `-- UnitTest1.cs
`-- PruebasUnitarias.slnx
```

## Requisitos

- SDK de .NET 10 instalado.

Verifica tu version con:

```bash
dotnet --version
```

## Restaurar, compilar y probar

Desde la raiz del repositorio:

```bash
dotnet restore .\PruebasUnitarias.slnx
dotnet build .\PruebasUnitarias.slnx
dotnet test .\PruebasUnitarias.slnx -c Release
```

## Ejecutar la API

```bash
dotnet run --project .\PruebasUnitarias.Api\PruebasUnitarias.Api.csproj
```

Endpoint principal:

- `GET /api/users`

## Cobertura de pruebas implementada

Las pruebas usan nomenclatura `Metodo_Escenario_ResultadoEsperado` y patron AAA (Arrange, Act, Assert):

1. `GetAsync_CuandoExistenUsuarios_RetornaOkConTresUsuarios`
   - Mock de `GetAllAsync` con 3 usuarios.
   - Valida `OkObjectResult` y cantidad exacta.
   - Verifica una sola llamada al servicio (`Times.Once`).

2. `GetByIdAsync_CuandoUsuarioExiste_RetornaOkConUsuarioIdUno`
   - Mock de `GetByIdAsync(1)`.
   - Valida `OkObjectResult` con `Id = 1`.
   - Verifica parametro correcto con `It.Is<int>(id => id == 1)`.

3. `GetByIdAsync_CuandoUsuarioNoExiste_RetornaNotFound`
   - Mock devolviendo `null`.
   - Valida `NotFoundResult`.

4. `CreateAsync_CuandoDatosValidos_RetornaCreatedAtActionYLlamaServicioUnaVez`
   - Ejecuta POST del controlador.
   - Valida `CreatedAtActionResult`.
   - Verifica `CreateAsync` una sola vez.

5. `CreateAsync_CuandoEmailEsJohnTestCom_VerificaParametroConItIs`
   - Verifica que el servicio reciba un usuario con `Email == "john@test.com"`.

6. `CreateAsync_CuandoSeEjecuta_CallbackCuentaUnaSolaLlamada`
   - Usa `Callback` para incrementar contador local.
   - Valida que se ejecuto exactamente una vez.

7. `DeleteAsync_CuandoUsuarioExiste_RetornaNoContentYEjecutaDelete`
   - Si el usuario existe, valida `NoContentResult`.
   - Verifica que `DeleteAsync` fue invocado.

8. `DeleteAsync_CuandoUsuarioNoExiste_RetornaNotFoundYNoEjecutaDelete`
   - Si el usuario no existe, valida `NotFoundResult`.
   - Verifica que `DeleteAsync` nunca fue invocado (`Times.Never`).

9. `FlujoCompleto_GetByIdCreateGetAll_CadaMetodoSeInvocaUnaVez`
   - Ejecuta flujo secuencial: `GetByIdAsync`, `CreateAsync`, `GetAsync`.
   - Verifica que cada metodo del mock fue llamado exactamente una vez.

## Notas

- `InMemoryUserService` existe para que la API compile y pueda ejecutarse sin dependencias externas.
- El foco del repositorio esta en pruebas unitarias de controlador con dobles de prueba (`Mock<IUserService>`), no en pruebas de integracion.
