# 3.0.0-rc.3
- Supports Swashbuckle 5, net core 3 and System.Text.Json
- Swashbuckle.AspNetCore updated to version 5.0.0

# 3.0.0-rc.2
- Supports net core 3 and brand new System.Text.Json
- NodaTime and NodaTime.Serialization.JsonNet updated to latest versions
- Swashbuckle.AspNetCore updated to version 5.0.0-rc5
- ConfigureForNodaTime became more customizable
- Compatibility with System.Text.Json:
  - New dependency: NodaTime.Serialization.SystemTextJson
  - Added new ConfigureForNodaTimeWithSystemTextJson
- Sample moved to net core 3
- Sample supports NewtonsoftJson, System.Text.Json and System.Text.Json with NamingPolicy from NewtonsoftJson

# 3.0.0-rc.1
- Swashbuckle.AspNetCore updated to version 5.0.0-rc4

# 2.0.0
- Swashbuckle.AspNetCore fixed to versions [4.0.1, 5.0.0)

# 1.2.0
- Swashbuckle.AspNetCore fixed to versions [2.4.0, 4.0.1)

# 1.1.0
- Dependencies updated

# 1.0.2
- Bugfix: Uses factory instead of shared Schema instance. See: PR#3

# 1.0.1
- Updated package description

# 1.0.0
- Implemented in c#, no FSharp.Core lib in dependencies
- JsonSerializerSettings ContractResolver uses for NamingStrategy, so you can use DefaultNamingStrategy, CamelCaseNamingStrategy or SnakeCaseNamingStrategy
- Added new DateInterval (use NodaTime.Serialization.JsonNet >= 2.1.0)

Full release notes can be found at: https://github.com/micro-elements/MicroElements.Swashbuckle.NodaTime/blob/master/CHANGELOG.md
