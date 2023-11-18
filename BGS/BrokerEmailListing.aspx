<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="BrokerEmailListing.aspx.cs" Inherits="BGS.BrokerEmailListing" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pagePannelHeader">
         <br />
        <br />

        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loader">
                            <div class="center">
                                <img alt="" src="Images/loader.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" CssClass="submitbutton" OnClick="SendEmail_Click" />

                <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="submitbutton" OnClick="" />--%>
                <input type="button" value="Cancel" name="Cancel" class="submitbutton" onclick="document.location.reload(true)" />
                <asp:Button ID="btnSaveUserEmailID" runat="server" Text="Save EmailID" CssClass="submitbutton" OnClick="btnSaveUserEmail_Click" />
                 <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="submitbutton" 
                            OnClick="btnExportExcel_Click" />
                <br />

                <asp:GridView ID="gvBrokerEmailList" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                    HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                    ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center" CssClass="mGrid" AlternatingRowStyle-CssClass="alt" Width="70%" Visible="true">
                    <Columns>
                         <asp:TemplateField HeaderText="Half Yearly">
                            <ItemTemplate>
                                <asp:TextBox ID="QuarterName" Width="100%" Text='<%#Eval("Name") %>' runat="server" />
                            </ItemTemplate>
                             </asp:TemplateField>
                        <asp:TemplateField HeaderText="Broker Name">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBrokerName" Width="100%" Text='<%#Eval("BrokerName") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Id">
                            <ItemTemplate>
                                <asp:TextBox ID="txtEmail" Width="100%" Text='<%#Eval("EmailID") %>' runat="server" OnTextChanged="txtEmail_TextChanged" AutoPostBack="true"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Status">
                            <ItemTemplate>
                                <asp:Label ID="txtEmailStatus" Text='<%#Convert.ToString(Eval("EmailStatus")).ToUpper() == "TRUE" ? "Mail Sent" : string.Empty%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkall" runat="server" Text="Send Mail-" TextAlign="Left" onclick="checkAll(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkEmail" runat="server" />
                                <asp:HiddenField ID="hfBrokerNameId" runat="server" Value='<%# Eval("BrokerNameID") %>' />
                                <asp:HiddenField ID="hfUserName" runat="server" Value='<%# Eval("UserName") %>' />                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
              <Triggers>
           
            <asp:PostBackTrigger ControlID="btnExportExcel" />
        </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="pagePannelHeader">
            <asp:Panel ID="Pdfpnl" runat="server" Visible="false">
                <table border="1" runat="server" id="tbl" style="width: 72%; border-collapse: collapse; text-decoration: none; font-family: Arial, Helvetica, sans-serif; font-size: 11px !important; text-align: justify;">
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
                            <asp:Label ID="lblReveiew" runat="server" Text="Period of Review:"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblReveiew1" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblReveiew2" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRank" runat="server" Text="Position of Rank:"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblRank1" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblRank2" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMov" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMov1" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMov2" runat="server"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3"><span><b>Note(1)</b>: As always for all meetings, you are requested to send a calendar invite to all attendees so that we have a record of all the meetings. Also please attach all reports and presentations to be discussed, in the calendar invite. Any new/ unlisted Company management meeting, pl attach detailed financials (Latest Quarterly results, Balance Sheet & cash flow statements) and Company presentation, if available, to enable preparation/ constructive discussion. </span></td>
                    </tr>
                    <tr>
                        <td colspan="3"><span><b>Note(2)</b>:Attached Tabular Information represents votes polled of Equity Funds & Equity Research, however Final Votes, Rankings & Brokerage are determined by the above as well as votes given by the Dealing Team.</span></td>
                    </tr>
                    <tr>
                        <td colspan="3"><span><b>Note(3)</b>:As always for all meetings, you are requested to send a calendar invite to all attendees so that we have a record of all the meetings. Also please attach all reports and presentations to be discussed, in the calendar invite. For any new/ unlisted Company management meeting, Please attach detailed financials (Latest Quarterly results, Balance Sheet & Cash Flow statements) and Company presentation, if available, to enable preparation/ constructive discussion. </span></td>
                    </tr>
                    <tr>
                        <td style="background-color: #F78181;">
                            <b>Broker/Firm:</b>
                        </td>
                        <td colspan="2" style="background-color: #F78181;">
                            <asp:Label ID="lblFirmName" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvBrokerRankReport" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                    HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                    ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center" CssClass="mGrid"
                    OnRowDataBound="gvBrokerRankReport_RowDataBound" AlternatingRowStyle-CssClass="alt" Width="72%">
                    <Columns>
                         <asp:BoundField HeaderText="Year" DataField="QuarterName" ItemStyle-Width="10%" />
                        <asp:BoundField HeaderText="Analyst Name" DataField="AnalystName" ItemStyle-Width="35%" />
                        <asp:BoundField HeaderText="Sector Name" DataField="SectorName" ItemStyle-Width="15%" />
                        <asp:BoundField HeaderText="Voters Vote" DataField="VoteShare" ItemStyle-Width="10%" DataFormatString="{0:F0}" NullDisplayText=""/>
                        <asp:BoundField HeaderText="Vote to Broker" DataField="TotalBrokerScore" ItemStyle-Width="10%" DataFormatString="{0:F0}"  />
                        <asp:BoundField HeaderText="Market Share" DataField="TotalSectorScore" ItemStyle-Width="10%"  DataFormatString="{0:F0}"  NullDisplayText="" />
                        <asp:BoundField HeaderText="No. of Analysts" DataField="TotalAnalyst" ItemStyle-Width="10%" />
                       <%-- <asp:TemplateField HeaderText="Year" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblQuarterName" runat="server" Text='<%# Eval("QuarterName").ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Analyst Name" DataField="AnalystName" ItemStyle-Width="25%" />
                        <asp:BoundField HeaderText="Sector Name" DataField="SectorName" ItemStyle-Width="25%" />
                        <%--  <asp:BoundField HeaderText="Vote" DataField="Vote" HeaderStyle-CssClass="gvActionHeadertxtAlign" />
                        <asp:BoundField HeaderText="Score" DataField="Score" HeaderStyle-CssClass="gvActionHeadertxtAlign" />
                        <asp:TemplateField HeaderText="Voter Share%" ItemStyle-Width="10%">
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
                        <%--<asp:BoundField HeaderText="Vote Share %" DataField="VoteShare" HeaderStyle-CssClass="gvActionHeadertxtAlign"  DataFormatString="{0:f%}"/>
                        <asp:BoundField HeaderText="Share %" DataField="TotalBrokerScore" HeaderStyle-CssClass="gvActionHeadertxtAlign"  DataFormatString="{0:f%}"/>
                        <asp:BoundField HeaderText="Sector Share %" DataField="TotalSectorScore" HeaderStyle-CssClass="gvActionHeadertxtAlign" DataFormatString="{0:f%}" />
                        <asp:BoundField HeaderText="No of Analyst in sector" DataField="TotalAnalyst" HeaderStyle-CssClass="gvActionHeadertxtAlign" />

                        <asp:TemplateField HeaderText="No of Analyst in sector" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblNoOfAnalyst" runat="server" Text='<%# Eval("TotalAnalyst").ToString()!="0" ? Eval("TotalAnalyst").ToString():""%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                    </Columns>
                </asp:GridView>

                 <asp:GridView ID="gridviewexcel" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                    HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                    ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center" CssClass="mGrid" AlternatingRowStyle-CssClass="alt" Width="100%">
                     <%--  <asp:GridView ID="gridviewexcel" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
                    HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                    ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center" CssClass="mGrid"
                    OnRowDataBound="gvBrokerRankReport_RowDataBound" AlternatingRowStyle-CssClass="alt" Width="72%">--%>

                      <Columns>
                         <asp:BoundField HeaderText="Half Yearly" DataField="Name" ItemStyle-Width="10%" />
                        <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" ItemStyle-Width="35%" />
                        <asp:BoundField HeaderText="Email Id" DataField="EmailID" ItemStyle-Width="15%" />
                        <asp:BoundField HeaderText="Email Status " DataField="EmailStatus" ItemStyle-Width="10%" DataFormatString="{0:F0}" NullDisplayText=""/>
                        
                    </Columns>
                       
                            
                     
                      
                </asp:GridView>

            </asp:Panel>
        </div>
    <script type="text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
</asp:Content>
