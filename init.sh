#!/bin/bash

# init.sh - Initialize the project and generate Asana API Clients with Kiota
#
# This script should only be run during initial setup or after a complete reset.
# For updates, use update.sh

set -e

echo "==================================="
echo "Panda.NuGet.AsanaClient - Init"
echo "==================================="
echo ""

# Check if Kiota is installed
if ! command -v kiota &> /dev/null
then
    echo "Error: Kiota is not installed."
    echo "Please install Kiota with:"
    echo "  dotnet tool install --global Microsoft.OpenApi.Kiota"
    exit 1
fi

# Check if Redocly is installed
if ! command -v redocly &> /dev/null
then
    echo "Error: Redocly CLI is not installed."
    echo "Please install Redocly with:"
    echo "  npm install -g @redocly/cli"
    exit 1
fi

# Change to project directory
cd "$(dirname "$0")/Panda.NuGet.AsanaClient"

echo "1. Downloading official Asana OpenAPI specification..."
curl -L -o asana_oas.yaml https://raw.githubusercontent.com/Asana/openapi/master/defs/asana_oas.yaml
echo "   ✓ Downloaded asana_oas.yaml"

echo ""
echo "2. Bundling and dereferencing OpenAPI spec with Redocly..."
redocly bundle asana_oas.yaml --dereferenced -o asana_flat.yaml
echo "   ✓ Created asana_flat.yaml"

echo ""
echo "3. Cleaning old Clients directory..."
if [ -d "Clients" ]; then
    rm -rf Clients
    echo "   ✓ Clients directory deleted"
fi

echo ""
echo "4. Generating API Clients with Kiota..."
kiota generate \
    -l CSharp \
    -d asana_flat.yaml \
    -n Panda.NuGet.AsanaClient.Clients \
    -o ./Clients \
    --clean-output

echo ""
echo "   ✓ API Clients successfully generated"

echo ""
echo "5. Restoring NuGet packages..."
cd ..
dotnet restore

echo ""
echo "6. Building project..."
dotnet build --configuration Release

echo ""
echo "==================================="
echo "✓ Initialization complete!"
echo "==================================="
echo ""
echo "Next steps:"
echo "  - For updates: ./update.sh"
echo "  - For build: dotnet build"
echo "  - For tests: dotnet test"
echo ""