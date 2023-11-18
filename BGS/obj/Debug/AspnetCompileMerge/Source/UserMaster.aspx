<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" EnableEventValidation="false" Inherits="BGS.UserMaster_Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%-- <style type="text/css">
        .tooltip
        {
            display: inline;
            position: relative;
            text-decoration: none;
            top: 0px;
            left: 4px;
        }
        .tooltip:hover:after
        {
            background: #333;
            background: rgba(0,0,0,.8);
            border-radius: 5px;
            top: -5px;
            color: #fff;
            content: attr(alt);
            left: 160px;
            padding: 5px 15px;
            position: absolute;
            z-index: 98;
            width: 150px;
        }
        .tooltip:hover:before
        {
            border: solid;
            border-color: transparent black;
            border-width: 6px 6px 6px 0;
            bottom: 20px;
            content: "";
            left: 155px;
            position: absolute;
            z-index: 99;
            top: 3px;
        }
    </style>--%>
    <script type="text/javascript" language="javascript">
        function SaveDate() {

            var v = $("input[id*='ctl00_ContentPlaceHolder1_rblType']:checked").val();

            if (v == "1") {
                var txtUserID = document.getElementById("ctl00_ContentPlaceHolder1_txtUserID");
                if (txtUserID != null) {
                    if (txtUserID.value == "") {
                        alert("Enter User Id");
                        return false;
                    }
                }
            }
            else {
                var txtOther = document.getElementById("ctl00_ContentPlaceHolder1_txtOther");
                if (txtOther != null) {
                    if (txtOther.value == "") {
                        alert("Enter User Id");
                        return false;
                    }
                }
            }

            var txtEmailID = document.getElementById("ctl00_ContentPlaceHolder1_txtEmailID");
            if (txtEmailID.value == "") {
                alert("Enter EmailID");
                return false;
            }

            var txtName = document.getElementById("ctl00_ContentPlaceHolder1_txtName");
            if (txtName.value == "") {
                alert("Enter Name");
                return false;
            }

            return true;
        }

        function SaveDateWithPassword() {

            var txtUserID = document.getElementById("ctl00_ContentPlaceHolder1_txtUserID");
            if (txtUserID.value == "") {
                alert("Enter EmployeeID");
                return false;
            }

            var txtEmailID = document.getElementById("ctl00_ContentPlaceHolder1_txtEmailID");
            if (txtEmailID.value == "") {
                alert("Enter EmailID");
                return false;
            }

            var txtName = document.getElementById("ctl00_ContentPlaceHolder1_txtName");
            if (txtName.value == "") {
                alert("Enter Name");
                return false;
            }
            return true;
        }

        function isNumericKeyStroke() {
            var returnValue = false;
            var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

            if (((keyCode >= 48) && (keyCode <= 57)) || ((keyCode >= 96) && (keyCode <= 105)) || // All numerics
                (keyCode == 8) ||     // Backspace
                (keyCode == 13))     // Carriage Return
                returnValue = true;

            if (window.event.returnValue)
                window.event.returnValue = returnValue;

            return returnValue;
        }

        //Cmt by shashi 09-08-2018
        //function ValidateEmail(email) {
        //    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        //    return expr.test(email);
        //};

        function ClientItemSelected(sender, e) {
            $get("<%=hfBrokerNameID.ClientID %>").value = e.get_value();
        }
        //Cmt by shashi 09-08-2018
        //$("[id*=btnSave]").live("click", function () {
        //    if (!ValidateEmail($("[id*=txtEmailID]").val())) {
        //        alert("Invalid email address.");
        //        return false;
        //    }
        //});
    </script>
   <script type="text/javascript">
       function checkValidationGroup(valGrp) {
           debugger;
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
           if (!checkValidationGroup("ValidUModule")) {
               //alert('Please fill required filled.');
               return false;
           }
           <%-- else if (s == 'YES') {
               <%-- var Start = document.getElementById('<%= hfStartDate.ClientID%>').value;
                var End = document.getElementById('<%= hfEndDate.ClientID%>').value;
                var Due = document.getElementById('<%= hfDueDate.ClientID%>').value;
                var StartDate = document.getElementById('<%= ddlStartDate.ClientID%>');
                var EndDate = document.getElementById('<%= ddlEndDate.ClientID%>');
                var DueDate = document.getElementById('<%= txtDueDate.ClientID%>');--%>
                <%-- if (confirm('Do you want to submit? because after submit you can not change.'))
                    return true;--%>
                <%-- else
                {
                    for (var i = 0; i < StartDate.options.length; i++) {
                        if (StartDate.options[i].text === Start) {
                            StartDate.selectedIndex = i;
                            break;
                        }
                    }
                    for (var i = 0; i < EndDate.options.length; i++) {
                        if (EndDate.options[i].text === End) {
                            EndDate.selectedIndex = i;
                            break;
                        }
                    }
                    document.getElementById('<%= txtDueDate.ClientID%>').value = Due;--%>
            <%-- else
                    return false;
                //}
                //return confirm('Do you want to submit? because after submit you can not change.');
            }--%>
       }
   </script>
    <asp:HiddenField runat="server" ID="hfBrokerNameID" />
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
            <div class="module">
                <%--<center>
                    <h3>Broker/Firm Master</h3>
                </center>--%>
            </div>
            <div style="text-align: center;">
                <div>
                    <br />
                    <div>
                        <table id="tblSearch" runat="server" class="table">
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearch" runat="server" Text="Search"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserSearch" runat="server" Placeholder="Email ID or EmployeeID Or Broker/Firm Name" CssClass="textbox" Width="300px" AutoPostBack="true" OnTextChanged="txtUserSearch_TextChanged"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                         <Ajax:AutoCompleteExtender ID="ACMEUSerSearch" runat="server" ServiceMethod="UserSearch" ServicePath="~/WebService/AutoComplete.asmx"
                                             MinimumPrefixLength="1"
                                             CompletionInterval="100" EnableCaching="true" CompletionSetCount="5"
                                             TargetControlID="txtUserSearch">
                                         </Ajax:AutoCompleteExtender>
                                </td>

                                <td>
                                    <asp:RadioButtonList ID="rdbtnl" runat="server" RepeatDirection="Horizontal" Width="250px"
                                        CellPadding="20" OnSelectedIndexChanged="rdbtnl_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Text="UTI"  Value="1" />
                                        <asp:ListItem Text="Broker/Firm" Value="0" />
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 300px">
                                    <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submitbutton" OnClick="btnreset_Click" />
                                </td>
                             
                                <td style="float: right;" colspan="15">
                                    <asp:Button ID="btnAddUser" runat="server" Text="Create User" CssClass="submitbutton"
                                        Width="131px" OnClick="CreateNewUser" />
                                </td>
                                <td>
                                     <asp:Button ID="btnExportExcel" Text="Export to Excel" OnClick="btnExportExcel_Click"

                                        runat="server" CssClass="submitbutton" Width="130" />
                                </td>
                            </tr>
                        </table>

                    </div>
                     
                    <table id="tblMain" class="table" width="100%">
                        <tr>
                            <td>
                                <asp:MultiView ID="mvTabs" runat="server">
                                    <asp:View ID="vwTab1" runat="server">
                                        <asp:GridView ID="grdUserRight" runat="server" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False"
                                            PageSize="20" Width="100%" ShowHeaderWhenEmpty="True" CssClass="mGrid" AllowPaging="false" EmptyDataRowStyle-HorizontalAlign="Center">
                                            <PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PageButtonCount="2"
                                                PreviousPageText="Previous" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="EmployeeID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("EmpID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--  <ItemStyle Width="25%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--  <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Role Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--  <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserType" runat="server" Text='<%# Eval("UserType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--   <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Broker Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBrokerName" runat="server" Text='<%# Eval("BrokerName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Weightage">
                                                    <ItemTemplate>
                                                        <asp:Label ID="iblweightage" runat="server" Text='<%# Eval("weight") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <ItemStyle Width="30%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkEdit" runat="server" CommandArgument='<%# Eval("EmpID")%>'
                                                            CssClass="submitbutton" Text="Edit" OnClick="EditUserDetails" />
                                                        |
                                                                <asp:Button ID="lnkStatus" runat="server" CommandArgument='<%# Eval("EmpID")%>'
                                                                    CssClass="submitbutton" Text='<%# Eval("Status")%>'
                                                                    OnClick="DeactivateUser" />
                                                        |
                                                                <asp:Button ID="lnkAssignScreen" runat="server" CommandArgument='<%# Eval("EmpID")%>'
                                                                    CommandName='<%# Eval("UserName") %>' CssClass="submitbutton"
                                                                    Text="Assign/Edit Screen" Width="120px" OnClick="AssignScreens" Visible="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle ForeColor="#FF3E3E" />
                                            <AlternatingRowStyle CssClass="alt" />
                                        </asp:GridView>

                                    </asp:View>
                                    <asp:View ID="vwTab2" runat="server">

                                        <table cellpadding="10" cellspacing="5">
                                            <tr id="trIsUserExists" runat="server" visible="false" class="error">
                                                <td colspan="3" align="center">
                                                    <asp:Label Text="Already Exist" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*"></asp:Label>

                                                </td>
                                                <td style="width: 100px;text-align: left">
                                                    <asp:Label ID="Label2" runat="server" Text="Login Type:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" CellPadding="20" ValidationGroup="UModule"
                                                        AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged"  style="align-items:flex-start">
                                                        <asp:ListItem Text="UTI" Value="1" Selected="True" />
                                                        <asp:ListItem Text="Broker/Firm" Value="0" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id="trUTI" runat="server">
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label4" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblUserID" runat="server" Text="User Id:" ToolTip="User ID" ></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtUserID" runat="server" OnTextChanged="txtUserID_TextChanged" AutoPostBack="true" CssClass="textbox" MaxLength="4" Width="230px" placeholder="Input User ID" ValidationGroup="UModule"></asp:TextBox>
                                                    <%--OnTextChanged="CheckIsExist" --%>
                                                    <asp:RequiredFieldValidator ID="rfvUserID" runat="server" ControlToValidate="txtUserID"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                    <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtUserID" />
                                                </td>
                                            </tr>
                                            <tr id="trOther" visible="false" runat="server">
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label3" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblOtherID" runat="server" Text="User Id:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtOther" runat="server" CssClass="textbox" OnTextChanged="CheckIsExist" AutoPostBack="true" MaxLength="50" Width="230px" placeholder="Input User Email ID"></asp:TextBox>
                                                    <%-- <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Numbers"
                                                        TargetControlID="txtOther" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName4" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblPassword" runat="server" Text="Password:" Visible="False"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" MaxLength="8" Width="230px"
                                                        TextMode="Password" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName5" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblEmailID" runat="server" Text="EmailID:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtEmailID" runat="server" OnTextChanged="CheckIsExist" AutoPostBack="true" CssClass="textbox" Width="230px" ValidationGroup="UModule"></asp:TextBox>                                                  
                                                     <asp:RequiredFieldValidator ID="rfvEmailID" runat="server" ControlToValidate="txtEmailID"
                                    InitialValue="" ErrorMessage="Required" ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                     <asp:RegularExpressionValidator ID="rEvEmail" runat="server" ControlToValidate="txtEmailID" ValidationGroup="ValidUModule"
                                                        ErrorMessage="Invalid Email" SetFocusOnError="true" Display="static" CssClass="ErrorMessage" Style="margin-left: -55px;"
                                                         ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName6" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName" runat="server" Text="Name:"  Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="230px" ValidationGroup="UModule"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                    InitialValue="" ErrorMessage="Required" ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName8" runat="server" Text="*" ForeColor="red"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblBrokerFirmname" runat="server" Text="Broker/Firm Name:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtBrokerName" runat="server" OnTextChanged="CheckIsExist" AutoPostBack="true" Width="230px" CssClass="textbox" ValidationGroup="UModule"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvBrokerName" runat="server" ControlToValidate="txtBrokerName"
                                    InitialValue="" ErrorMessage="Required" ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                   <%-- <Ajax:AutoCompleteExtender ServiceMethod="PopulateBrokerName" ServicePath="~/WebService/AutoComplete.asmx"
                                                        MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                                        CompletionInterval="100" EnableCaching="true" CompletionSetCount="5"
                                                        TargetControlID="txtBrokerName"
                                                        ID="AutoCompleteExtender1" runat="server">
                                                    </Ajax:AutoCompleteExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblCode" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblDofaCode" runat="server" Text="Dofa Code:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtDofaCode" runat="server" OnTextChanged="CheckIsExist" AutoPostBack="true" Width="230px" CssClass="textbox"></asp:TextBox>
                                                    <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtDofaCode" />
                                                    <%--<Ajax:AutoCompleteExtender ServiceMethod="PopulateBrokerName" ServicePath="~/WebService/AutoComplete.asmx"
                                                        MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                                        CompletionInterval="100" EnableCaching="true" CompletionSetCount="5"
                                                        TargetControlID="txtBrokerName"
                                                        ID="AutoCompleteExtender2" runat="server">
                                                    </Ajax:AutoCompleteExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblMaxlimit18" runat="server" Text="*" ForeColor="red"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblMaxLimit" runat="server" CssClass="" Text="Max Limit:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtMaxLimit" runat="server" CssClass="textbox" Width="230px" ValidationGroup="UModule"></asp:TextBox>
                                                     <asp:RequiredFieldValidator ID="rfvMaxLimit" runat="server" ControlToValidate="txtMaxLimit"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName7" runat="server" Text="*" ForeColor="red"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblRole" runat="server" Text="Role:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdowncss" Width="230px"
                                                        ValidationGroup="UMaster">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName9" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblDept" runat="server" Text="Department:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlDept" runat="server" CssClass="dropdowncss" Width="230px"
                                                        ValidationGroup="UMaster">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvDept" runat="server" ControlToValidate="ddlDept"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName12" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblUserType" runat="server" Text="Type:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:DropDownList ID="ddlUserType" runat="server" CssClass="dropdowncss" Width="230px"
                                                        ValidationGroup="UMaster">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvUserType" runat="server" ControlToValidate="ddlUserType"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr runat="server" id="trWeight">
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblName11" runat="server" ForeColor="red" Text="*"></asp:Label>
                                                </td>
                                                <td style="text-align: left">

                                                    <asp:Label ID="lblWeight" runat="server" Text="Weight:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtWeight" runat="server" CssClass="textbox" Width="230px" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ControlToValidate="txtWeight"
                                    InitialValue="" ErrorMessage="Required." ForeColor="Red" ValidationGroup="ValidUModule"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblIsActive" runat="server" Text="IsActive:"></asp:Label>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="checkBoxcss" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="left">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trButton">
                                                <td colspan="3" style="text-align: center">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="submitbutton" ValidationGroup="ValidUModule"
                                                        Text="Save" OnClick="SaveUserDetails" CausesValidation="true" />
                                                    <%--OnClientClick="SaveDate();"--%>
                                                    &nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" runat="server" CssClass="submitbutton"
                                                    Text="Back" OnClick="CancelUserDetails" />

                                                </td>
                                            </tr>
                                            <tr id="tdUTI2" style="display: none;">
                                                <td align="center" style="width: 100%" colspan="3">
                                                    <asp:Button ID="Button1" runat="server" CssClass="submitbutton"
                                                        Text="Assign Screen" />
                                                </td>
                                            </tr>
                                        </table>


                                    </asp:View>
                                </asp:MultiView>
                            </td>
                        </tr>
                    </table>
                     
                </div>
            </div>

            <asp:GridView ID="gvExcel" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found."

                AlternatingRowStyle-CssClass="alt" Width="100%" ShowHeaderWhenEmpty="True" CssClass="mGrid"

                EmptyDataRowStyle-HorizontalAlign="Center" Visible="true">

                <%--<PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PageButtonCount="2" PreviousPageText="Previous" />--%>

                <Columns>

                    <asp:BoundField HeaderText="EmployeeID" DataField="EmpID" />

                    <asp:BoundField HeaderText="Name" DataField="UserName" HtmlEncode="true" />

                    <asp:BoundField HeaderText="Role Name" DataField="RoleName" HtmlEncode="false" />

                    <asp:BoundField HeaderText="User Type" DataField="UserType" />

                    <asp:BoundField HeaderText="Department" DataField="DeptName" />

                    <asp:BoundField HeaderText="Email ID" DataField="EmailID" />
                      <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />
                      <asp:BoundField HeaderText="Weightage" DataField="Weight" />

                  

                      <%--  <ItemTemplate>

                            <asp:Label ID="lblMaxLimit" Text='<%# Eval("MaxLimit") %>' runat="server" />

                            <asp:HiddenField runat="server" ID="hfBrokerNameIDEdit" Value='<%#Eval("BrokerNameID") %>' />

                        </ItemTemplate>--%>

                

                    <%--<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">

                        <ItemTemplate>

 

                            <asp:Button ID="btnEdit" runat="server" CssClass="submitbutton" Height="30px" Width="50px" Text="Edit" OnClick="EditBroker" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName='<%#Eval("BrokerID") %>' />

                            <asp:Button ID="btnDelete" runat="server" CssClass="submitbutton" Height="30px" Width="50px" Text="Delete" CommandArgument='<%#Eval("BrokerID") %>' OnClientClick='return confirm("Do you want to delete this broker ?");' OnClick="DeleteBrokerMaster" />

                        </ItemTemplate>

                    </asp:TemplateField>--%>

                </Columns>

                <PagerStyle ForeColor="#FF3E3E" />

                <AlternatingRowStyle CssClass="alt" />

           </asp:GridView>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>--%>
        <Triggers>

            <asp:PostBackTrigger ControlID="btnExportExcel" />

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
