# Define variables
PROJECT_NAME = LuzInga.Api
DOTNET = dotnet
PROFILE = LuzInga.Api
# Define build targets
build:
	$(DOTNET) build $(PROJECT_NAME)/$(PROJECT_NAME).csproj

clean:
	$(DOTNET) clean $(PROJECT_NAME)/$(PROJECT_NAME).csproj

# Define run target
run:
	$(DOTNET) watch run --project $(PROJECT_NAME)/$(PROJECT_NAME).csproj --launch-profile $(PROFILE)
