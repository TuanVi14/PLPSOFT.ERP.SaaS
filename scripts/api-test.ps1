$base = 'http://localhost:5183'
function Get-Json([string]$path){
    try{
        $r = Invoke-RestMethod -Uri ($base + $path) -Method GET -ErrorAction Stop
        Write-Output "--- $path ---"
        $r | ConvertTo-Json -Depth 5
    }catch{
        Write-Output "ERROR calling $path : $($_.Exception.Message)"
    }
}
Get-Json '/api/suppliers'
Get-Json '/api/suppliergroups'
