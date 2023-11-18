<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="BrokerGradingReports.aspx.cs" Inherits="BGS.BrokerGradingReports" %>

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
                    <%-- <td>
                        <asp:Button ID="btnExcel1" runat="server" Text="Export to excel1" CssClass="submitbutton" OnClick="btnExcel1_Click" />&nbsp;&nbsp;
                    </td>--%>
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
        <div class="pagePannelHeader">
            <table id="tdlogo" runat="server" visible="false">
                <tr>
                    <td valign="top">
                        <img src='<%=ResolveClientUrl("~/Images/uti_mutual_fund.jpg") %>' alt="" title="UTI MF Logo">
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvBGSReport" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                ShowHeaderWhenEmpty="True" CssClass="mGrid" Width="100%" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField HeaderText="Quarter Name" DataField="QuarterName" />
                    <asp:BoundField HeaderText="Voter Name" DataField="VoterName" />
                    <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />
                    <asp:BoundField HeaderText="DOFA Code" DataField="BrokerDofaCode" />
                    <asp:BoundField HeaderText="Analyst Name" DataField="AnalystName" />
                    <%--<asp:TemplateField HeaderText="Analyst Name">
                        <ItemTemplate>
                            <asp:Label ID="lblAnalystName" runat="server" Text='<%# Eval("AnalystName").ToString()%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField HeaderText="Sector Name" DataField="SectorName" />
                    <asp:BoundField HeaderText="Vote" DataField="Vote" />
                    <asp:BoundField HeaderText="Score" DataField="Score" />
                    <asp:BoundField HeaderText="TotalBrokerScore" DataField="TotalBrokerScore" />
                    <asp:TemplateField HeaderText="Voter Share%">
                        <ItemTemplate>
                            <asp:Label ID="lblVoterShare" runat="server" Text='<%# Eval("VoteShare").ToString()!=""? Eval("VoteShare").ToString()+"%":null%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Share%">
                        <ItemTemplate>
                            <asp:Label ID="lblShare" runat="server" Text='<%# Eval("TotalBrokerScore").ToString()!="" ? Eval("TotalBrokerScore").ToString()+"%":null%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText=" Sector Share%">
                        <ItemTemplate>
                            <asp:Label ID="lblSectorShare" runat="server" Text='<%# Eval("TotalSectorScore").ToString()!="" ? Eval("TotalSectorScore").ToString()+"%":null%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No of Analyst">
                        <ItemTemplate>
                            <asp:Label ID="lblNoOFAnalyst" runat="server" Text='<%# Eval("TotalAnalyst").ToString()!="0" ? Eval("TotalAnalyst").ToString():null%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%-- <asp:BoundField HeaderText="No of Analyst in sector" DataField="TotalAnalyst" />--%>
                    <%--<asp:BoundField HeaderText="Vote Share %" DataField="VoteShare" DataFormatString="{0:f}" />--%>
                    <%--  <asp:BoundField HeaderText="Share % " DataField="TotalBrokerScore" DataFormatString="{0:Ff}" />
                    <asp:BoundField HeaderText="Sector Share %" DataField="TotalSectorScore" DataFormatString="{0:f}" />--%>
                    <%-- <asp:TemplateField >
                   <ItemTemplate>
                    <asp:HiddenField ID="hfBrokerNameID"  runat="server" Value='<%#Eval("BrokerNameID")%>'/>
                     <asp:HiddenField ID="hfQuarterID"  runat="server" Value='<%#Eval("QuarterID")%>'/>
                   </ItemTemplate>
               </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </div>


      

    </div>
    <br />
    <br />
</asp:Content>
