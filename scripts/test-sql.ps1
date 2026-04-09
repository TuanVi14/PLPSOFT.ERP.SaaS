$cs = "Data Source=HUSHNOW\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True;"
Add-Type -AssemblyName System.Data
$conn = New-Object System.Data.SqlClient.SqlConnection($cs)
try {
    $conn.Open()
    Write-Output "SQL-CONN-OK"
    $conn.Close()
} catch {
    Write-Output "SQL-CONN-ERROR: $($_.Exception.Message)"
}
