# Release Process

This document describes how to create a new release and publish to NuGet.org.

## Prerequisites

Before you can publish, you need to set up the NuGet API key as a GitHub secret:

### 1. Get Your NuGet API Key

1. Go to [NuGet.org](https://www.nuget.org/)
2. Sign in to your account
3. Go to **API Keys** under your profile
4. Click **Create** to create a new API key
5. Configure the key:
   - **Key Name**: `Panda.NuGet.AsanaClient GitHub Actions`
   - **Glob Pattern**: `Panda.NuGet.AsanaClient`
   - **Scopes**: Select "Push new packages and package versions"
   - **Expiration**: Choose an appropriate expiration (recommend 365 days)
6. Click **Create**
7. **Copy the API key immediately** (you won't see it again!)

### 2. Add API Key to GitHub Secrets

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Create a secret:
   - **Name**: `NUGET_API_KEY`
   - **Value**: Paste your NuGet API key
5. Click **Add secret**

## Creating a Release

### Option 1: Via GitHub UI (Recommended)

1. Go to your repository on GitHub
2. Click on **Releases** (right sidebar)
3. Click **Draft a new release**
4. Fill in the release information:
   - **Choose a tag**: Create a new tag with format `v1.0.0` (must start with 'v')
   - **Release title**: `v1.0.0 - Initial Release` (or describe the release)
   - **Description**: Add release notes describing changes
5. Click **Publish release**

This will automatically:
- Create the git tag
- Trigger the GitHub Actions workflow
- Download and bundle the latest Asana OpenAPI spec
- Generate API clients with Kiota
- Build the project
- Create the NuGet package with the version from the tag
- Publish to NuGet.org
- Attach the package files to the GitHub release

### Option 2: Via Command Line

```bash
# Make sure you're on the main branch and up to date
git checkout main
git pull

# Create and push a version tag
git tag v1.0.0
git push origin v1.0.0

# The GitHub Actions workflow will automatically run
```

Then go to GitHub to add release notes and finalize the release.

## Version Numbering

Follow [Semantic Versioning](https://semver.org/):

- **MAJOR** version (v1.0.0 → v2.0.0): Breaking changes
- **MINOR** version (v1.0.0 → v1.1.0): New features, backwards compatible
- **PATCH** version (v1.0.0 → v1.0.1): Bug fixes, backwards compatible

Examples:
- `v1.0.0` - Initial release
- `v1.0.1` - Bug fix
- `v1.1.0` - New feature (backwards compatible)
- `v2.0.0` - Breaking changes

## Workflow Details

The publish workflow (`publish-nuget.yml`) does the following:

1. **Triggered by**: Pushing a tag matching `v*.*.*`
2. **Extracts version**: Removes the 'v' prefix from tag (v1.0.0 → 1.0.0)
3. **Downloads OpenAPI spec**: Gets latest from official Asana GitHub
4. **Bundles with Redocly**: Creates dereferenced specification
5. **Generates clients**: Uses Kiota to generate API clients
6. **Updates version**: Modifies .csproj with tag version
7. **Builds project**: Compiles in Release configuration
8. **Runs tests**: Executes tests if they exist
9. **Creates package**: Generates .nupkg and .snupkg files
10. **Publishes to NuGet**: Uploads to NuGet.org
11. **Creates GitHub Release**: Attaches package files

## Monitoring the Release

After pushing a tag:

1. Go to **Actions** tab in your GitHub repository
2. Find the "Publish to NuGet" workflow run
3. Monitor the progress
4. Check for any errors

The workflow typically takes 2-3 minutes to complete.

## Verifying the Release

After the workflow completes:

1. Check [NuGet.org](https://www.nuget.org/packages/Panda.NuGet.AsanaClient/)
2. The new version should appear within a few minutes
3. It may take up to 15 minutes for the package to be indexed and searchable

## Testing a Pre-Release

To test the release process without publishing:

1. Create a tag like `v1.0.0-beta1` or `v1.0.0-rc1`
2. The workflow will run and create a pre-release package
3. Pre-release packages won't appear by default in NuGet searches

## Troubleshooting

### Workflow fails with "401 Unauthorized"

- Check that `NUGET_API_KEY` secret is set correctly
- Verify the API key hasn't expired
- Ensure the API key has "Push" permissions

### Version already exists on NuGet

- You cannot republish the same version
- Create a new tag with an incremented version number
- The workflow uses `--skip-duplicate` to prevent errors

### Build fails

- Check the workflow logs in the Actions tab
- Ensure all prerequisites (Kiota, Redocly) install correctly
- Verify the Asana OpenAPI spec URL is accessible

### Package not appearing on NuGet

- Wait up to 15 minutes for indexing
- Check the NuGet.org package page directly
- Verify the workflow completed successfully

## Manual Publishing (Fallback)

If the automated workflow fails, you can publish manually:

```bash
# Run the update script to generate clients
./update.sh

# Update version in .csproj manually
# Edit Panda.NuGet.AsanaClient/Panda.NuGet.AsanaClient.csproj
# Change <Version>1.0.0</Version> to your version

# Build and pack
dotnet pack --configuration Release --output ./nupkg

# Publish to NuGet
dotnet nuget push ./nupkg/Panda.NuGet.AsanaClient.*.nupkg \
    --api-key YOUR_NUGET_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

## Rollback

If you need to unlist a bad release:

1. Go to [NuGet.org](https://www.nuget.org/)
2. Navigate to your package
3. Select the version
4. Click **Unlist** (this hides it from search but existing users can still use it)

Note: You cannot delete a version from NuGet.org.

## Continuous Integration

Every push to `main` or `develop` branches triggers the `build-and-test.yml` workflow:

- Validates that the code builds successfully
- Generates clients from the latest OpenAPI spec
- Runs tests
- Creates a package artifact (not published)

This ensures the codebase is always in a releasable state.
