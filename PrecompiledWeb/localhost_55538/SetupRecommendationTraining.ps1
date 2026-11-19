$ErrorActionPreference = 'Stop'

$directory = Join-Path $PSScriptRoot 'App_Data'
$database = Join-Path $directory 'RecommendationTraining.accdb'
if (Test-Path -LiteralPath $database) {
	throw "The training database already exists: $database. It will not be overwritten."
}
if (-not (Test-Path -LiteralPath $directory)) {
	New-Item -ItemType Directory -Path $directory | Out-Null
}

$builder = New-Object System.Data.OleDb.OleDbConnectionStringBuilder
$builder['Provider'] = 'Microsoft.ACE.OLEDB.12.0'
$builder['Data Source'] = $database.ToString()
$catalog = New-Object -ComObject ADOX.Catalog
try {
	[void]$catalog.Create($builder.ConnectionString)
	$catalog.ActiveConnection.Close()
}
finally {
	[void][System.Runtime.InteropServices.Marshal]::FinalReleaseComObject($catalog)
}

$connection = New-Object System.Data.OleDb.OleDbConnection($builder.ConnectionString)
try {
	$connection.Open()
	$command = $connection.CreateCommand()
	try {
		$command.CommandText = 'CREATE TABLE [RecommendationGroups] ([TrackingId] TEXT(100) NOT NULL CONSTRAINT [PK_RecommendationGroups] PRIMARY KEY, [HasSeasonalPick] YESNO NOT NULL)'
		[void]$command.ExecuteNonQuery()
		$command.CommandText = 'INSERT INTO [RecommendationGroups] ([TrackingId], [HasSeasonalPick]) VALUES (?, ?)'
		[void]$command.Parameters.Add('?', [System.Data.OleDb.OleDbType]::VarWChar, 100)
		[void]$command.Parameters.Add('?', [System.Data.OleDb.OleDbType]::Boolean)
		foreach ($record in @(@('route-48a', $true), @('route-93c', $false), @('browse-home', $false))) {
			$command.Parameters[0].Value = $record[0]
			$command.Parameters[1].Value = $record[1]
			[void]$command.ExecuteNonQuery()
		}
	}
	finally {
		$command.Dispose()
	}
}
finally {
	$connection.Dispose()
}
Write-Output "Created dummy training database: $database"
