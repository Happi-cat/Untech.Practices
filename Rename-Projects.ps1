param(
	[string]$what,
	[string]$with
)

$entryDir = pwd

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

function Replace-InCodeFiles {
	gci *.csproj,*.sln,*.cs -Recurse | %{
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


cd $entryDir; Replace-InSubFolderNames src
cd $entryDir; Replace-InSubFolderNames test
cd $entryDir; Replace-InSubFolderNames examples
cd $entryDir; Replace-InCodeFiles
cd $entryDir