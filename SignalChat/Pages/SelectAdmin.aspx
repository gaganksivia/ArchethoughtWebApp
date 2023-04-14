<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Main.Master" AutoEventWireup="true" CodeBehind="SelectAdmin.aspx.cs" Inherits="SignalChat.Pages.SelectAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="modal fade" role="dialog" tabindex="-1" id="admin-list-modal">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-dialog modal-login">
                    <div class="modal-content">
                        <div class="modal-header" style="padding: 0px;">
                            <h4 class="h4 modal-title">Choose your buddy</h4>
                        </div>
                        <div class="modal-body">
                            <form id="user-login-form" method="post">
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row no-gutters">
                                                <asp:Literal runat="server" ID="ltOnlineAdmin" />
                                        </div>
                                    </div>
                                </div>
                                <div class="d-flex justify-content-center form-group" id="user-login-submit" style="margin-top: 1REM;">
                                    <asp:Button class="btn btn-primary btn-block btn-lg" runat="server" ID="btnStartChat" Text="Start Chat" OnClick="btnStartChat_Click" />
                                    <asp:HiddenField runat="server" ID="hfselectedAdmin" />
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
