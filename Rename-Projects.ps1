param(
	[string]$what,
	[string]$with,
	[string]$where = (pwd)
)

function Replace-InFile($file) {
	$content = (gc $file);
	$hasMatches = $content | ?{ $_.Contains($what) }
	if ($hasMatches) {
		write-host "Replacing in file $_" -ForegroundColor Green
		$content | %{ $_ -replace $what, $with } | out-file $file -Encoding UTF8
	}
}

function Replace-InFileName($file) {
	$file = gi $file;
	if ($file.Name.Contains($what)) {
		cd $file.Directory
		$newFile = ($file.Name -replace $what, $with)
		write-host "Renaming $($file.Name) into $newFile"  -ForegroundColor Green
		mv $file.Name $newFile
	}
}

function Replace-InCodeFiles($dir) {
	gci $dir -include *.csproj,*.cs -exclude 'TemporaryGenerated*' -Recurse | ?{
		-not $_.FullName.Contains('/bin/') -and -not $_.FullName.Contains('/obj/')
	} | %{
		Replace-InFile -file $_
		Replace-InFileName -file $_
	}
}

function Replace-InSubFolderNames($dir) {
	gci $dir -Directory | ?{ $_.Name.Contains($what) } | %{
		cd "$($_.FullName)\.."
		$newDir = ($_.Name -replace $what, $with)
		write-host "Moving dir $($_.Name) to $newDir"  -ForegroundColor Green
		mv $_.Name $newDir
	}
}

@( 'src', 'test', 'examples', 'tools' ) | %{ join-path $where $_ } | %{
	Write-host "Processing $_..." -ForegroundColor Green
	Replace-InSubFolderNames $_
	Replace-InCodeFiles $_
}

Write-host "Processing solution files..." -ForegroundColor Green
gci $where -include *.sln -recurse | %{
	Replace-InFile $_ 
}
cd $where