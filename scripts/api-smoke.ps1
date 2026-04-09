$base = 'http://localhost:5183'
$controllers = @('suppliers','suppliergroups')
function Test-Endpoint($name){
    $url = "$base/api/$name"
    try{
        $r = & 'curl.exe' -k -s -S -L $url -w "\nHTTP:%{http_code}\n"
        Write-Output "== $url =="
        if([string]::IsNullOrEmpty($r)){
            Write-Output "(empty response)"
        } else {
            # show only first 1000 chars
            $body = $r -replace '\r','' -replace '\n',' '

            if($body.Length -gt 1000){ $sample = $body.Substring(0,1000) + '...'; } else { $sample = $body }
            Write-Output $sample
        }
    } catch {
        Write-Output "ERROR calling $url : $($_.Exception.Message)"
    }
}
foreach($c in $controllers){ Test-Endpoint $c }
