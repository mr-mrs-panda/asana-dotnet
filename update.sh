#!/bin/bash

# update.sh - Update the generated Asana API Clients
#
# This script should be run when the OpenAPI specification has changed
# and the generated clients need to be updated.

set -e

echo "==================================="
echo "Panda.NuGet.AsanaClient - Update"
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

echo "1. Downloading latest official Asana OpenAPI specification..."
curl -L -o asana_oas.yaml https://raw.githubusercontent.com/Asana/openapi/master/defs/asana_oas.yaml
echo "   ✓ Downloaded asana_oas.yaml"

echo ""
echo "2. Bundling and dereferencing OpenAPI spec with Redocly..."
redocly bundle asana_oas.yaml --dereferenced -o asana_flat.yaml
echo "   ✓ Created asana_flat.yaml"

echo ""
echo "3. Updating API Clients with Kiota..."
kiota generate \
    -l CSharp \
    -d asana_flat.yaml \
    -n Panda.NuGet.AsanaClient.Clients \
    -o ./Clients \
    --clean-output

echo ""
echo "   ✓ API Clients successfully updated"

echo ""
echo "4. Restoring NuGet packages..."
cd ..
dotnet restore

echo ""
echo "5. Building project..."
dotnet build --configuration Release

echo ""
echo "==================================="
echo "✓ Update complete!"
echo "==================================="
echo ""
echo "The generated clients in the 'Clients' directory have been updated."
echo ""