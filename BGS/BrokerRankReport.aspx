﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeBehind="BrokerRankReport.aspx.cs" Inherits="BGS.BrokerRankReport" %>

<%@ Import Namespace="BGS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .header
        {
            background-color: #425290;
            font-family: Calibri;
            color: White;
            text-align: center;
        }
    </style>
    <div>
        <%-- <asp:Panel ID="PAdd" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center" class="headertxt pagePannelHeader">
            <div>
                Broker Rank
            </div>
        </asp:Panel>--%>
        <br />
        <br />
        <div class="pagePannelHeader">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblQuarter" runat="server" Text="Half Yearly"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="dropdowncss">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblBrokerName" runat="server" Text="Broker/Firm"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBrokerName" runat="server" CssClass="dropdowncss" Width="300px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="submitbutton" OnClick="btnReset_Click" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submitbutton" OnClick="btnSearch_Click" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnExportPdf" runat="server" Text="Export to pdf" CssClass="submitbutton"
                            OnClick="btnExportPdf_Click" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" CssClass="submitbutton"
                            OnClick="btnSendEmail_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="pagePannelHeader">
            <asp:Panel ID="Pdfpnl" runat="server" Visible="false">
                <table border="1" runat="server" id="tbl" style="width: 100%; border-collapse: collapse;
                    text-decoration: none; font-family: Arial, Helvetica, sans-serif; font-size: 11px !important;
                    text-align: justify;">
                    <%-- <tr>
                        <td colspan="3" style="text-align:center; font-weight:800; font:900;" >
                        <h4>Per Broker/Firm Rank Report</h4>
                    </td>
                    </tr>--%>
                    <tr>
                        <td id="tdlogo" runat="server" visible="false" colspan="3" style="text-align: left">
                            <img src="./Images/uti_mutual_fund.jpg" alt="" title="UTI MF Logo" width="50px">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblReveiew" Style="font-weight: normal" runat="server" Text="Period of Review:"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblReveiew1" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblReveiew2" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRank" Style="font-weight: normal" runat="server" Text="Position of Rank:"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblRank1" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblRank2" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMov" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMov1" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMov2" Style="font-weight: bold" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdNote1" colspan="3">
                            <span style="color: green; font-size: 10pt"><b>Note(1)</b>:'Market Share' represents market share for your analyst for that
                                sector only. Please note that our analyst's vote is not necessarily equally weighted
                                across the different sectors covered by them. Thus for example, if Cement and FMCG
                                are covered by the same analyst, he/she may choose to assign her votes in a 30:70 ratio,
                                indicating his/her priorities for that vote-period. Other votes are given by Fund Managers.
                                The last column indicates the number of analysts voted for on the sell-side in a
                                sector. </span>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdNote2" colspan="3">
                            <span style="color: green; font-size: 10pt"><b>Note(2)</b>: Attached Tabular Information represents votes polled of Equity
                                Funds & Equity Research, however Final Votes, Rankings & Brokerage are determined
                                by the above as well as votes given by the Dealing Team.</span>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdNote3" colspan="3">
                            <span style="color: green; font-size: 10pt"><b>Note(3)</b>:As always for all meetings, you are requested to send a calendar
                                invite to all attendees so that we have a record of all the meetings. Also please
                                attach all reports and presentations to be discussed, in the calendar invite. For
                                any new/ unlisted Company management meeting, www available, to enable preparation/
                                constructive discussion. </span>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="background-color: #F78181;">
                            <b>Broker/Firm:</b>
                        </td>
                        <td colspan="2" style="background-color: #F78181;">
                            <asp:Label ID="lblFirmName" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>--%>
                </table>
                <asp:GridView ID="gvBrokerRankReport" runat="server" EmptyDataText="No Row Found."
                    AutoGenerateColumns="false" HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center"
                    HorizontalAlign="Center" ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center"
                    CssClass="mGrid" OnRowDataBound="gvBrokerRankReport_RowDataBound" AlternatingRowStyle-CssClass="alt"
                    Width="100%" OnRowCreated="gvBrokerRankReport_RowCreated" ShowHeader="false">
                    <Columns>
                        <asp:BoundField HeaderText="Year" DataField="QuarterName" ItemStyle-Width="10%" />
                        <asp:BoundField HeaderText="Analyst Name" DataField="AnalystName" ItemStyle-Width="25%" />
                         <asp:BoundField HeaderText="Voter Comment" DataField="RMComment" ItemStyle-Width="40%" />
                        <asp:BoundField HeaderText="Sector Name" DataField="SectorName" ItemStyle-Width="12%" />
                        <asp:BoundField HeaderText="Voters Vote" DataField="VoteShare" ItemStyle-Width="4%"
                            DataFormatString="{0:F0}" NullDisplayText="" />
                        <asp:BoundField HeaderText="Vote to Broker" DataField="TotalBrokerScore" ItemStyle-Width="4%"
                            DataFormatString="{0:F0}" />
                        <asp:BoundField HeaderText="Market Share" DataField="TotalSectorScore" ItemStyle-Width="3%"
                            DataFormatString="{0:F0}" NullDisplayText="" />
                        <asp:BoundField HeaderText="No. of Analysts" DataField="TotalAnalyst" ItemStyle-Width="12%" />
                        <%--<asp:TemplateField HeaderText="Year" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblQuarterName" runat="server" Text='<%# Eval("QuarterName").ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Voter Share" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblVoterShare" runat="server" Text='<%# Eval("VoteShare").ToString()!=""? System.Math.Round(Convert.ToDecimal(Eval("VoteShare").ToString()),0)+"%":null%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Share%" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblBrokerShare" runat="server" Text='<%# Eval("TotalBrokerScore").ToString()!="" ? System.Math.Round(Convert.ToDecimal(Eval("TotalBrokerScore").ToString()),0)+"%":null%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Share of Sector%" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblSectorShare" runat="server" Text='<%# Eval("TotalSectorScore").ToString()!="" ? System.Math.Round(Convert.ToDecimal(Eval("TotalSectorScore").ToString()),0)+"%":null%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="No of Analyst in sector" ItemStyle-Width="10%">
                            <ItemTemplate>
                               <asp:Label ID="lblNoOfAnalyst" runat="server" Text='<%# Eval("TotalAnalyst").ToString()!="0" ? Eval("TotalAnalyst").ToString():""%>'></asp:Label>
                               <%-- <asp:Label ID="lblNoOfAnalyst" runat="server" Text='<%# Eval("TotalAnalyst")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <br />
        <br />
    </div>
</asp:Content>
