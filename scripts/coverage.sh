cd ..
cd tools
dotnet restore
 
# Instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --workdir ../ --assemblies tests/**/bin/**/*.dll --sources src/**/*.cs --exclude-sources src/**/Program.cs --exclude-sources src/**/Startup.cs --exclude-sources src/**/DataAccess/**/*.cs --exclude-sources src/**/Migrations/**/*.cs
 
# Reset hits count in case minicover was run for this project
dotnet minicover reset
 
cd ..
 
for project in tests/**/*.csproj; do dotnet test --no-build $project; done
 
cd tools
 
# Uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument --workdir ../
 
 threshold=80
# Create HTML reports inside folder coverage-html
# This command returns failure if the coverage is lower than the threshold
dotnet minicover htmlreport --workdir ../ --threshold $threshold
 
# Print console report
# This command returns failure if the coverage is lower than the threshold
dotnet minicover report --workdir ../ --threshold $threshold
 
cd ..