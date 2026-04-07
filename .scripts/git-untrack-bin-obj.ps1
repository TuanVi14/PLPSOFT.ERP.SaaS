Set-Location 'C:\Users\acer\PLPSOFT.ERP.SaaS'
if (-not (Test-Path .gitignore)) { New-Item .gitignore -ItemType File -Force | Out-Null }
$content = ''
try { $content = Get-Content .gitignore -Raw } catch { $content = '' }
if ($content -notmatch '\*\*/bin/') { Add-Content .gitignore '**/bin/' }
if ($content -notmatch '\*\*/obj/') { Add-Content .gitignore '**/obj/' }
$tracked = git ls-files | Select-String -Pattern '/bin/|/obj/' | ForEach-Object { $_.Line }
if ($tracked -and $tracked.Count -gt 0) {
  foreach ($f in $tracked) { git rm --cached --ignore-unmatch -- $f }
  git add .gitignore
  git commit -m 'Remove tracked build artifacts and ignore bin/obj'
} else {
  git add .gitignore
  git commit -m 'Add bin/obj to .gitignore'
}
Write-Output 'git-untrack-bin-obj: done'
