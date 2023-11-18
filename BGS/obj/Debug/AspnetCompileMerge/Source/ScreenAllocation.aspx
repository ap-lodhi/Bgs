<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="ScreenAllocation.aspx.cs" Inherits="BGS.ScreenAllocation_Page" %>
<%@ Register src="~/UserControls/ScreenAllocation.ascx" tagname="ScreenAllocation" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <uc1:ScreenAllocation ID="ucScreen" runat="server" Visible="false" />
    
</asp:Content>
