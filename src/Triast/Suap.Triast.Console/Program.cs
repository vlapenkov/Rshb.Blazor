// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var str = new string('a', 1_000);

var chArr = str.ToCharArray();



chArr[10] = 'b';

Console.WriteLine(new String(chArr));

Console.ReadKey();

var sb =new StringBuilder();

var ls =str.ToList();

Span<char> mySpan = stackalloc char[str.Length]; // or `new char[str.Length]`

str.AsSpan().CopyTo(mySpan);

//MemoryMarshal.GetReference(str.To)

mySpan[10] = 'i';

Console.WriteLine(mySpan.ToString());

Console.ReadKey();


long val1 = 100;
string str1 = "silmple string";
DateOnly dt1 = new DateOnly(2024,01,15);
bool bRet = true;

string  serVal1 = JsonSerializer.Serialize(val1);
string serVal2 = JsonSerializer.Serialize(str1);
string serVal3 = JsonSerializer.Serialize(dt1);
string serVal4 = JsonSerializer.Serialize(bRet);


var res1 =JsonSerializer.Deserialize<long>(serVal1);
var res2 = JsonSerializer.Deserialize<string>(serVal2);
var res3 = JsonSerializer.Deserialize<DateOnly>(serVal3);
var res4 = JsonSerializer.Deserialize<bool>(serVal4);

Console.WriteLine(res1);
Console.WriteLine(res2);
Console.WriteLine(res3);
Console.WriteLine(res4);
Console.ReadLine();

