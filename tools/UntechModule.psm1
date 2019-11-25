function Set-Vs2017Env {
	$env:Path += ";C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\"
}

function Set-Vs2019Env {
	$env:Path += ";C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\"
}

function Sync-GitRepo {
	param(
		[string]$workDir = (pwd).path
	)

	$pwd = (pwd).path;

	if ( Test-Path (join-path $workDir '.git') ) {
		write-host "Syncing $workDir" -foregroundcolor green
		cd $workDir
		git fetch -p
		Remove-GitGoneBranches
	}

	if ( Test-Path (join-path $workDir '.syncable-gits') ) {
		$gitRepos = gc (join-path $workDir '.syncable-gits')

		$gitRepos | %{ (join-path $workDir $_) } | %{ Sync-GitRepo $_ }
	}

	cd $pwd
}

function Install-Software {
	param(
		[string]$softwareListFile = ".software-list"
	)

	$packages = gc $softwareListFile | %{ $_ -replace "#.+", "" } | %{ $_.Trim() } | ?{ $_ }

	foreach ($package in $packages) {
		choco install $package -y
	}
}

function Update-Software {
	param(
		[string]$softwareListFile = ".software-list"
	)

	$packages = gc $softwareListFile | %{ $_ -replace "#.+", "" } | %{ $_.Trim() } | ?{ $_ }

	foreach ($package in $packages) {
		choco upgrade $package -y
	}
}

function Remove-GitGoneBranches {
	$branches = git branch -v | Select-String -Pattern "\[gone\]" | %{ $_ -replace "^\s*([\w/-]+)\s.+$", '$1' }

	foreach ($branch in $branches) {
		git branch -D $branch
	}
}


function Remove-BuildFolders {
	[cmdletbinding(SupportsShouldProcess=$true)]
	param([switch]$Force)

	gci -Directory | %{ 
		(join-path $_.Fullname "bin"), (join-path $_.Fullname "obj")
	} | ?{
		test-path $_
	} |%{
		write-verbose "Deleting $_"
		rm $_ -Recurse -Force:$Force
	}
}

function Get-RandomName {
	param(
		$count = 10
	)

	if ($count -lt 1) { $count = 1; }

	$vowels = @('A','E','I','O','U')
	$consonants = @('B','C','D','F','G','H','J','K','L','M','N','P','Q','R','S','T','V','W','X','Y','Z')

	$vowels = $vowels + ($vowels | %{ $_.ToLower() })
	$consonants = $consonants + ($consonants | %{ $_.ToLower() })

	$symbols = 1..$count | %{ $_ % 2 } | %{
		if ($_ -eq 1) { $consonants | Get-Random }
		else { $vowels | Get-Random }
	}

	return -join $symbols
}

function Get-RandomSentence {
	param($wordsCount = 5, 
		$minWordSize = 4,
		$maxWordSize = 15
	)

	if ($wordsCount -lt 1) { $wordsCount = 1; }

	$words = 1..$wordsCount | %{ Get-Random -Minimum $minWordSize -Maximum $maxWordSize } | %{
		Get-RandomName -count $_
	}

	return $words -join ' '
}

function Get-RandomParagraph {
	param($sentenceCount = 4,
		$minSentenceSize = 5,
		$maxSentenceSize = 15
	)

	if ($paragraphCount -lt 1) { $paragraphCount = 1; }

	$sentences = 1..$sentenceCount | %{ Get-Random -Minimum $minSentenceSize -Maximum $maxSentenceSize } | %{
		Get-RandomSentence -wordsCount $_
	} | %{ "$_." }

	return "  " + ($sentences -join ' ')
}

Export-ModuleMember -Function Set-Vs2017Env
Export-ModuleMember -Function Set-Vs2019Env
Export-ModuleMember -Function Sync-GitRepo
Export-ModuleMember -Function Install-Software
Export-ModuleMember -Function Update-Software
Export-ModuleMember -Function Remove-GitGoneBranches
Export-ModuleMember -Function Remove-BuildFolders
Export-ModuleMember -Function Get-RandomName
Export-ModuleMember -Function Get-RandomSentence
Export-ModuleMember -Function Get-RandomParagraph