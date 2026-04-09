$cs = "Data Source=HUSHNOW\SQLEXPRESS02;Initial Catalog=PLPSOFT_ERP_SALES_SAAS_V2026;Integrated Security=True;"
Add-Type -AssemblyName System.Data
$conn = New-Object System.Data.SqlClient.SqlConnection($cs)
$conn.Open()
$queries = @(
    'SELECT COUNT(*) FROM Companies',
    'SELECT COUNT(*) FROM SystemTypes',
    'SELECT COUNT(*) FROM SystemTypeValues',
    'SELECT COUNT(*) FROM ProductUnits',
    'SELECT COUNT(*) FROM Products',
    'SELECT COUNT(*) FROM CustomerGroups',
    'SELECT COUNT(*) FROM Customers',
    'SELECT COUNT(*) FROM CustomerAddresses'
)
foreach($q in $queries){
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $q
    $r = $cmd.ExecuteScalar()
    Write-Output "$q => $r"
}
$conn.Close()
