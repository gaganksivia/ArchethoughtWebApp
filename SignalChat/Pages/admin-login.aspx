<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Main.Master" AutoEventWireup="true" CodeBehind="admin-login.aspx.cs" Inherits="SignalChat.Pages.admin_login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <section style="padding: 4rem;">
        <div class="container bg-dark py-5">
            <div class="row d-flex justify-content-center">
                <div class="col-md-6 col-xl-4 login-form">
                    <div>
                        <p class="fw-bold text-center text-success mb-2">Secured by Signal&nbsp;<span style="font-weight: normal !important; color: rgb(188, 192, 195); background-color: rgb(32, 33, 36);">©</span><br><br></p>
                        <h2 class="fw-bold text-center">Admin Login</h2>
                        <form id="admin-login-form" class="p-3 p-xl-4" method="post">
                            <div class="mb-3"><input class="form-control" runat="server" type="text" id="username" name="username" placeholder="Username" required="required"></div>
                            <div class="mb-3"><input class="form-control" type="password" runat="server" id="password" name="password" placeholder="Password" required="required"></div>
                            <div class="mb-3"></div>
                            <div>
                                <asp:Button ID="btnLogin"  class="btn btn-primary shadow d-block w-100"  runat="server" Text="Login" OnClick="btnLogin_Click" /></div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <footer class="bg-dark">
        <div class="container py-4 py-lg-5">
            <hr>
            <div class="text-muted d-flex justify-content-between align-items-center pt-3">
                <p class="mb-0">Copyright © 2023 Archethought Messaging Service</p>
            </div>
        </div>
    </footer>
</asp:Content>
