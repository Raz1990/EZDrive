<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminControlPanel.aspx.cs" Inherits="adminControlPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Label">time in SECONDS</asp:Label>
        <asp:TextBox ID="timeSeconds" runat="server"></asp:TextBox>
        <asp:Button runat="server" ID="btn" Text="start global Timer" onclick="btn_Start"/><br /><br />
        <asp:Button runat="server" ID="Button2" Text="end global Timer" onclick="btn_End"/><br />
    
        <asp:Button ID="btnDef" runat="server" Text="default 5 min" OnClick="btnDef_Click" />
    </div>
    </form>
</body>
</html>
