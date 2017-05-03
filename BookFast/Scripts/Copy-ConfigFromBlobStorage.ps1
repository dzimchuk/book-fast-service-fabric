Param(
    [String]$destination,
    [String]$connectionString,
    [String]$containerName,
    [String]$prefix
    )

$context = New-AzureStorageContext -ConnectionString $connectionString

Get-AzureStorageContainer -Context $context -Name $containerName | Get-AzureStorageBlob -Prefix $prefix | % {
    if (!(Test-Path $destination))
    {
        New-Item -Path $destination -ItemType directory -Force
    }
    
    $fileName = Join-Path $destination (Split-Path $_.Name -Leaf)
    Write-Host $fileName
    
    Get-AzureStorageBlobContent -Context $context -Container $containerName -Blob $_.Name -Destination $fileName -Force | Out-Null
}