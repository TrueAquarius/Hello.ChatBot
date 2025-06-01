# Define the path to the file
$filePath = ".\BuildInfo.cs"
Write-Host $filePath

# Read all lines from the file
$fileContent = Get-Content -Path $filePath

# Get the current date and time in the desired format
$newBuildDate = (Get-Date).ToString("dd.MM.yyyy HH:mm K")

# Define regex patterns for BuildNumber and BuildDate
$buildNumberPattern = 'public const int BuildNumber = (\d+);'
$buildDatePattern = 'public const string BuildDate = ".*";'

# Initialize variables to store the updated content
$updatedContent = @()

# Loop through each line to update the required fields
foreach ($line in $fileContent) {
    if ($line -match $buildNumberPattern) {
        # Extract the current build number, increment it, and update the line
        $currentBuildNumber = [int]$matches[1]
        $newBuildNumber = $currentBuildNumber + 1
        $updatedContent += $line -replace $buildNumberPattern, "public const int BuildNumber = $newBuildNumber;"
    } elseif ($line -match $buildDatePattern) {
        # Update the BuildDate line with the current date
        $updatedContent += $line -replace $buildDatePattern, "public const string BuildDate = `"$newBuildDate`";"
    } else {
        # Keep other lines unchanged
        $updatedContent += $line
    }
}

# Write the updated content back to the file
$updatedContent | Set-Content -Path $filePath

Write-Host "BuildInfo updated successfully!"