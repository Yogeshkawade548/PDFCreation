

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="PDFCreation.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="align-content: center">
            <div style="width: 430px">
                <asp:Button ID="Button1" runat="server" Text="Create PDF Document With Text." OnClick="Button1_Click" Width="431px" /></div>
            <div style="width: 430px">
                <asp:Button ID="Button2" runat="server" Text="Create PDF Document With Image." OnClick="Button2_Click" Width="430px" /></div>
            <div style="width: 430px">
                <asp:Button ID="Button3" runat="server" Text="Create PDF Document With Grid." OnClick="Button3_Click" Width="429px" /></div>
            <div style="width: 430px">
                <asp:Button ID="Button4" runat="server" Text="Create PDF Document in Landscape Orientation." OnClick="Button4_Click" Width="429px" /></div>
        </div>
    </form>
</body>
</html>
