# Projekty w solucji

1. MainApp.Web - MVC projekt
- middleware
- account Register/Login/Logout
- trainers viewer
- events viewer
- controllers
2. MainApp.BLL
- context
- entity
- enums
- migration Msql, Postgress, Mysql, SqlLite MainAppUserDb.db only user with identity
- models
- repository
- Services
3. Tracking Api
- DataBase Msql, postgress, mysql, SqlLite MainAppDb.db trainers and events
- context
- controllers
- migration
- models
- repository
- Services
4. MainAppIntegrationTests
- test index request
5. MainAppIntegrationMVCTests
- mvc tests
6. MainAppUnitTests
- testing trainer controller
- testing user service
7. SendEmail.API
- testing to send mail using MailKit

## Start projektu
1. prawy przycisk na solucji "Set Startup Projects"
2. wybieramy MainApp.Web i Tracking
3. Start

## Unit tests
1. Wybieramy dla projektow Tracking i MainWeb env. "UnitTests"