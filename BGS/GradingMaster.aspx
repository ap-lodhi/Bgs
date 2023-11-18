<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeBehind="GradingMaster.aspx.cs" Inherits="BGS.GradingMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BGS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script type="text/jscript" src="JS/JScript.js"></script>--%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script type="text/javascript" language="javascript">
        function ConfirmMessage() {
            debugger;                  
            var selectedvalue = confirm("Do you want to proceed for new start date and end Date.if you proceed then you can not change last start date and end date data ?");
            if (selectedvalue) {
                document.getElementById('<%=hfQuarter.ClientID %>').value = "Yes";
            } else {               
                document.getElementById('<%=hfQuarter.ClientID %>').value = "No";
            }
            
        }
    </script>
    <script type="text/javascript">
        function Check_Click(objRef) {

            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }
    </script>
    <script type="text/javascript">
        function checkAllDepts(objRef) {
            $("[id*=chkDept]").each(function () {
                $(this).attr('checked', $(objRef).is(":checked"));
            })
        }

        function EnterEvent(e) {
            if (e.keyCode == 13) {
                __doPostBack('<%=btnDept.UniqueID%>', "");
            }
        }        
    </script>
    <script type="text/javascript">
        function checkValidationGroup(valGrp) {
            var rtnVal = true;
            for (i = 0; i < Page_Validators.length; i++) {
                if (Page_Validators[i].validationGroup == valGrp) {
                    ValidatorValidate(Page_Validators[i]);
                    if (!Page_Validators[i].isvalid) { //at least one is not valid.
                        rtnVal = false;
                        break; //exit for-loop, we are done.
                    }
                }
            }
            return rtnVal;
        }
        function performCheck(s) {
            debugger;
            if (!checkValidationGroup("A")) {
                alert('Please fill required filled.');
                return false;
            }
            else if (s == 'YES') {
                debugger;
                var Start = document.getElementById('<%= hfStartDate.ClientID%>').value;
                var End = document.getElementById('<%= hfEndDate.ClientID%>').value;
                var Due = document.getElementById('<%= hfDueDate.ClientID%>').value;
                var StartDate = document.getElementById('<%= ddlStartDate.ClientID%>');
                var EndDate = document.getElementById('<%= ddlEndDate.ClientID%>');
                var DueDate = document.getElementById('<%= txtDueDate.ClientID%>');
                if (confirm('Do you want to submit? because after submit you can not change.'))
                    return true;
                else
                {
                    for (var i = 0; i < StartDate.options.length; i++) {
                      //  alert('1');
                        if (StartDate.options[i].text === Start) {
                         //   alert('2');
                            StartDate.selectedIndex = i;
                            break;
                        }
                    }
                    for (var i = 0; i < EndDate.options.length; i++) {
                        if (EndDate.options[i].text === End) {
                            EndDate.selectedIndex = i;
                         //   alert('3');
                            break;
                        }
                    }
                    document.getElementById('<%= txtDueDate.ClientID%>').value = Due;
                    return false;
                }
                //return confirm('Do you want to submit? because after submit you can not change.');
            }
        }
    </script>
    <style type="text/css">
        .PnlDesign
        {
            border: solid 1px #000000;
            height: 150px;
            width: 200px;
            overflow-y: scroll;
            background-color: #EAEAEA;
            font-size: 15px;
            font-family: Arial;
            padding-left: 5px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <div>
                <div class="module">
                    <center>
                        MANAGE GRADING SCREEN</center>
                </div>
                <div>
                    <div>
                        <table class="table">
                            <tr>
                                <%--<td>
                                    Select Quarter
                                </td>
                                <td>
                                    <asp:HiddenField ID="hfQuarter" runat="server" />
                                    <asp:TextBox ID="txtQuarter" runat="server" CssClass="textbox" OnTextChanged="txtQuarter_TextChanged"
                                        AutoPostBack="true" AutoComplete="false" />
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtQuarter"
                                        MaskType="Number" Mask="LL_9999_L9" MessageValidatorTip="true" InputDirection="LeftToRight"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="false">
                                    </cc1:MaskedEditExtender>
                                    <asp:HiddenField ID="hfQuarterId" runat="server" />
                                    <asp:RequiredFieldValidator ErrorMessage="*" CssClass="errorMessage" ValidationGroup="A"
                                        ControlToValidate="txtQuarter" InitialValue="" runat="server" />
                                </td>--%>
                                
                                <td>Start Date
                                </td>
                                <td> 
                                    <asp:HiddenField ID="hfQuarter" runat="server" />
                                    <asp:HiddenField ID="hfStartDate" runat="server" />
                                    <asp:HiddenField ID="hfEndDate" runat="server" />
                                     <asp:HiddenField ID="hfQuarterId" runat="server" />
                                    <asp:HiddenField ID="hfDueDate" runat="server" />

                                    <asp:DropDownList ID="ddlStartDate" runat="server" CssClass="dropdowncss" >  <%--OnSelectedIndexChanged="ddlStartDate_SelectedIndexChanged"--%>
                                        <%--<asp:ListItem Selected="True" Text=""></asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ErrorMessage="*" CssClass="errorMessage" ValidationGroup="A"
                                        ControlToValidate="ddlStartDate" InitialValue="" runat="server" />

                                    
                                </td>
                                <td>End Date 
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEndDate" runat="server" CssClass="dropdowncss" >  <%--OnSelectedIndexChanged="ddlEndDate_SelectedIndexChanged"--%>
                                        <%--<asp:ListItem Selected="True" Text=""></asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ErrorMessage="*" CssClass="errorMessage" ValidationGroup="A"
                                        ControlToValidate="ddlEndDate" InitialValue="" runat="server" />
                                </td>
                                
                                <td>
                                    Due Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDueDate" runat="server" CssClass="textbox"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtfrmdate_CalendarExtender" runat="server" Enabled="True"
                                        Format="dd-MMM-yyyy" TargetControlID="txtDueDate" PopupButtonID="Image6">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton ID="Image6" runat="Server" ImageUrl="~/images/Calendar_scheduleHS.png"
                                        Height="17px" Width="17px" ImageAlign="AbsMiddle" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" CssClass="errorMessage"
                                        ValidationGroup="A" ControlToValidate="txtDueDate" InitialValue="" runat="server" />
                                </td>
                                <td><asp:Button Text="Go" CssClass="Gobutton" OnClick="Go_Click" OnClientClick="return performCheck('YES');"
                                        ValidationGroup="A" runat="server" /></td>
                                <td>
                                    <asp:Button Text="Calculate Rank" CssClass="submitbutton" Style="float: right" OnClick="CalculateRank"
                                        ValidationGroup="A" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDept" Text="Select Department" runat="server" onkeypress="return EnterEvent(event)"
                                        CssClass="textbox"></asp:TextBox>
                                    <asp:Panel ID="PnlDept" runat="server" CssClass="PnlDesign" Style="display: none">
                                        <asp:CheckBox ID="chkAll" Text="Select All" runat="server" onclick="checkAllDepts(this);" />
                                        <asp:CheckBoxList ID="chkDept" runat="server" Width="150px" TextAlign="Right">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:Button ID="btnDept" runat="server" CssClass="submitbutton" Text="Submit" OnClick="DeptWiseRecord" />
                                    <br />
                                    <cc1:PopupControlExtender ID="PceSelectCustomer" runat="server" TargetControlID="txtDept"
                                        PopupControlID="PnlDept" Position="Bottom">
                                    </cc1:PopupControlExtender>
                                    <%--  <asp:DropDownList ID="ddlDept" runat="server" Width="150px">
                            </asp:DropDownList>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <div>
                        <asp:GridView ID="gvGradingrMaster" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found."
                            EmptyDataRowStyle-HorizontalAlign="Center" DataKeyNames="GradingID" ShowHeaderWhenEmpty="True"
                            CssClass="mGrid" Width="100%" lternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:TemplateField HeaderText="User Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" Text='<%# Eval("UserName") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Role" DataField="RoleName" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="gvActionHeadertxtAlign" />
                                <asp:BoundField HeaderText="Department" DataField="Department" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="gvActionHeadertxtAlign" />
                                <asp:TemplateField HeaderText="Email-Id" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" Text='<%# Eval("EmailId") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Status" DataField="StatusName" ConvertEmptyStringToNull="true"
                                    NullDisplayText="Blank" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkall" runat="server" Text="Send Mail-" TextAlign="Left" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--                                <asp:CheckBox ID="CheckBox1" Checked='<%# Eval("IsConfirm") %>' Visible='<%# Eval("IsSend").ToString().ToLower() == "False".ToLower() %>' runat="server" onclick = "Check_Click(this);" />--%>
                                        <asp:CheckBox ID="chkIsConfirm" Checked='<%# Eval("IsConfirm") %>' runat="server"
                                            onclick="Check_Click(this);" TextAlign="Left" />
                                        <%--                                <asp:Label Text="Already Email Send" Visible='<%# Eval("IsSend").ToString().ToLower() == "True".ToLower() %>' runat="server" />--%>
                                        <asp:HiddenField ID="hfIsSend" runat="server" Value='<%# Eval("IsSend") %>' />
                                        <asp:HiddenField ID="hfEmpId" runat="server" Value='<%# Eval("EmpID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ConvertEmptyStringToNull="true"
                                    HeaderStyle-CssClass="gvActionHeadertxtAlign">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtn" Text="Re-Open" OnClientClick='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("StatusName"))) ? (Eval("StatusName").ToString().ToLower() != EnumStatus.Closed.ToString().ToLower() ? "return false" : "return confirm(&#39;Do you want to Re-open this vote ?&#39;);") : "return false"%>'
                                            OnClick="ReopenEditGrading_Click" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("StatusName"))) ? (Eval("StatusName").ToString().ToLower() != EnumStatus.Closed.ToString().ToLower() ? false : true) : false  %>'
                                            CausesValidation="false" CssClass="btn btn-link" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex%>"
                                            CommandName='<%#Eval("GradingID")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div>
                            <div style="width: 60%">
                                <asp:Button ID="btnCancel" runat="server" OnClick="Cancel" Text="Cancel" CssClass="submitbutton"
                                    Style="float: right" />&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnVoting" Text="Initiate voting" runat="server" CausesValidation="true"
                                    ValidationGroup="A" CssClass="submitbutton" Style="float: right" OnClick="SaveGradingMaster_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDept" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
</asp:Content>
