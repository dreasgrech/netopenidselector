<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OpenIDSelectorImplementation._Default" %>




<%@ Register assembly="OpenIDSelector" namespace="OpenIDSelector" tagprefix="OpenIDSelector" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <OpenIDSelector:NETOpenIDSelector ID="NETOpenIDSelector1" runat="server" 
            LoadjQuery="true" OnSelectorChosen="Selected" />
        
    </form>
</body>
</html>