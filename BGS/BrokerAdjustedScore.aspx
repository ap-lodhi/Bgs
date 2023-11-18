<%@ Page Language="C#"  MasterPageFile="~/MasterPages/MasterPage.master" 
    AutoEventWireup="true" CodeBehind="BrokerAdjustedScore.aspx.cs" Inherits="BGS.BrokerAdjustedScore" %>

<%@ Import Namespace="BGS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
        <br />
        <br />
     <div class="pagePannelHeader">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblQuarter" runat="server" Text="Quarter "></asp:Label>&nbsp;&nbsp;
                    </td>
                   <%-- <td>
                        <asp:Label ID="lblDepartmentName" runat="server" Text="Department "></asp:Label>
                        &nbsp;&nbsp;
                    </td>--%>
                    <td>
                        <asp:Label ID="lblBrokerName" runat="server" Text="Broker/Firm"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                   
                    
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                   <%-- <td>
                        <asp:DropDownList ID="ddlDepartmentName" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>--%>
                    
                    <td>
                        <asp:DropDownList ID="ddlBrokerName" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submitbutton" OnClick="btnSearch_Click" />
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
                        <asp:Button ID="btnPDF" runat="server" Text="Export to pdf" CssClass="submitbutton" OnClick="btnPdf_Click" />
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <br />



      <div class="pagePannelHeader">
            <table id="tdlogo" runat="server" visible="false">
                <tr>
                    <td valign="top">
                        <img src='<%=ResolveClientUrl("D:/Tejarmayee/UTI Development Projects/BGS_Latest/BGS/Images/uti_mutual_fund.jpg") %>' alt="" title="UTI MF Logo" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvBGSReport" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                ShowHeaderWhenEmpty="True" CssClass="mGrid" Width="100%" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField HeaderText="Quarter Name" DataField="QuarterName" />
                    <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />

                    <%--<asp:BoundField HeaderText="Weighted score of DFM & DOSR" DataField="AdjustedScore_DFM_DOSR" />
                    <asp:BoundField HeaderText="Broker Adjusted Score of DFM & DOSR" DataField="AdjustedWeightedScore_DFM_DOSR" />

                    <asp:BoundField HeaderText="Weighted score of DOD" DataField="AdjustedScore_DOD" />
                    <asp:BoundField HeaderText="Broker Adjusted Score of DOD" DataField="AdjustedWeightedScore_DOD" />

                    <asp:BoundField HeaderText="Weighted score of DOFA" DataField="AdjustedScore_DOFA" />
                    <asp:BoundField HeaderText="Broker Adjusted Score of DOFA" DataField="AdjustedWeightedScore_DOFA" />--%>

                    <asp:TemplateField HeaderText="Weighted score of DFM & DOSR">
                        <ItemTemplate>
                            <asp:Label ID="lblWeightedScore_DFM_DOSR" runat="server" Text='<%# Eval("AdjustedScore_DFM_DOSR").ToString()!="0.00"? Eval("AdjustedScore_DFM_DOSR").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Broker Adjusted Score of DFM & DOSR">
                        <ItemTemplate>
                            <asp:Label ID="lblAdjustedWeightedScore_DFM_DOSR" runat="server" Text='<%# Eval("AdjustedWeightedScore_DFM_DOSR").ToString()!="0.00"? Eval("AdjustedWeightedScore_DFM_DOSR").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

<%--                       <asp:BoundField HeaderText="Weighted score of DOD" DataField="AdjustedScore_DOD" />
                    <asp:BoundField HeaderText="Broker Adjusted Score of DOD" DataField="AdjustedWeightedScore_DOD" />

                    <asp:BoundField HeaderText="Weighted score of DOFA" DataField="AdjustedScore_DOFA" />
                    <asp:BoundField HeaderText="Broker Adjusted Score of DOFA" DataField="AdjustedWeightedScore_DOFA" />--%>

                     <asp:TemplateField HeaderText="Weighted score of DOD">
                        <ItemTemplate>
                            <asp:Label ID="lblAdjustedScore_DOD" runat="server" Text='<%# Eval("AdjustedScore_DOD").ToString()!="0.00"? Eval("AdjustedScore_DOD").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Broker Adjusted Score of DOD">
                        <ItemTemplate>
                            <asp:Label ID="lblAdjustedWeightedScore_DOD" runat="server" Text='<%# Eval("AdjustedWeightedScore_DOD").ToString()!="0.00"? Eval("AdjustedWeightedScore_DOD").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Weighted score of DOFA">
                        <ItemTemplate>
                            <asp:Label ID="lblAdjustedScore_DOFA" runat="server" Text='<%# Eval("AdjustedScore_DOFA").ToString()!="0.00"? Eval("AdjustedScore_DOFA").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Broker Adjusted Score of DOFA">
                        <ItemTemplate>
                            <asp:Label ID="lblAdjustedWeightedScore_DOFA" runat="server" Text='<%# Eval("AdjustedWeightedScore_DOFA").ToString()!="0.00"? Eval("AdjustedWeightedScore_DOFA").ToString()+"":"-"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>



    </div>
</asp:Content>