<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true"  CodeBehind="AdjustedWeightageReports.aspx.cs" Inherits="BGS.AdjustedWeightageReports" %>

<%@ Import Namespace="BGS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div class="module">
            <%--<center>
                    <h3>Broker/Firm Master</h3>
                </center>--%>
        </div>
             <div class="pagePannelHeader">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblQuarter" runat="server" Text="Quarter"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblvoterName" runat="server" Text="Voter Name"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblSectorName" runat="server" Text="Sector Name"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblBrokerName" runat="server" Text="Broker/Firm"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="submitbutton" OnClick="btnReset_Click" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnExcell" runat="server" Text="Export to excel" CssClass="submitbutton" OnClick="btnExcell_Click" />&nbsp;&nbsp;
                    </td>
                     <td>
                        <asp:Button ID="btnExcel1" runat="server" Text="Export to excel1" CssClass="submitbutton" OnClick="btnExcel1_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlvoterName" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSector" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBrokerName" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submitbutton" OnClick="btnSearch_Click" />
                        &nbsp;&nbsp;
                        
                    </td>

                    <td>
                        <asp:Button ID="btnPDF" runat="server" Text="Export to pdf" CssClass="submitbutton" OnClick="btnPdf_Click" />
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>

         <div>
                <asp:GridView ID="gvBGSAdjustedWeight" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                ShowHeaderWhenEmpty="True" CssClass="mGrid" Width="100%" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField HeaderText="Voter Name" DataField="UserName" />
                    <asp:BoundField HeaderText="Department Name" DataField="Department Name" />
                    <asp:BoundField HeaderText="Weights" DataField="each_voter_sum" />
                    <asp:BoundField HeaderText="Adjusted Weights" DataField="adjusted_weight" />
                <%--    <asp:HiddenField ID="hfBrokerNameID"  runat="server" Value='<%#Eval("BrokerNameID")%>'/>
                     <asp:HiddenField ID="hfQuarterID"  runat="server" Value='<%#Eval("QuarterID")%>'/>
                 --%>
                </Columns>
            </asp:GridView>
        </div>

        </div>
</asp:Content>