param(
	[string]$what,
	[string]$with
)

function Replace-InFile($file) {
	$content = (gc $file); 
	$content | %{ $_ -replace $what, $with } | out-file $file -Encoding UTF8
}

gci *.csproj,*.sln,*.cs -Recurse | ?{ 
    (gc $_) | ?{ $_.Contains($what) } 
} | %{
    write-host "Replacing in file $_" -ForegroundColor Green
    Replace-InFile -file $_  -what $what -with $with
}

gci src -Directory | ?{
    $_.Name.Contains($what)
} | %{
    cd "$($_.FullName)\.."
    $newDir = ($_.Name -replace $what, $with)
    write-host "Moving dir $($_.Name) to $newDir"  -ForegroundColor Green
    mv $_.Name $newDir
}

gci *.csproj,*.sln,*.cs -Recurse | ?{
    $_.Name.Contains($what)
} | %{ 
    cd $_.Directory
    $newFile = ($_.Name -replace $what, $with)
    write-host "Renaming $($_.Name) into $newFile"  -ForegroundColor Green
    mv $_.Name $newFile
}
