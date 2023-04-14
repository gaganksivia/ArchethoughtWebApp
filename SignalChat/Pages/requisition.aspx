<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Main.Master" AutoEventWireup="true" CodeBehind="requisition.aspx.cs" Inherits="SignalChat.Pages.requisition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="py-5">
        <div class="container py-5">
            <div class="row mb-5">
                <div class="col-md-8 col-xl-6 text-center mx-auto">
                    <h2 class="fw-bold">Tell us what you need</h2>
                </div>
            </div>
            <div class="row row-cols-1 row-cols-md-2 mx-auto" style="max-width: 900px;">
                <div class="col mb-4 requisition-type">
                    <div class="req-type">
                        <a href="#">
                            <img class="rounded img-fluid shadow w-100 fit-cover req-type-image" src="/img/products/depositphotos_98505198-stock-photo-fresh-vegetables-fruits-and-other.jpg" style="height: 250px;"></a>
                        <div class="py-4">
                            <span class="badge bg-primary mb-2">Secure</span>
                            <h4 class="fw-bold">Food</h4>
                            <input type="hidden" value="1" />
                            <p class="text-muted">You can request food assistance by filling out a simple form. Our goal is to ensure that everyone has access to the food they need to thrive, and we are committed to providing compassionate, responsive support to all who request it. </p>
                        </div>
                    </div>
                </div>
                <div class="col mb-4 requisition-type">
                    <div class="req-type">
                        <a href="#">
                            <img class="rounded img-fluid shadow w-100 fit-cover req-type-image" src="/img/products/istockphoto-1177156986-612x612.jpg" style="height: 250px;"></a>
                        <div class="py-4">
                            <span class="badge bg-primary mb-2">Secure</span>
                            <h4 class="fw-bold">Shelter</h4>
                            <input type="hidden" value="2" />
                            <p class="text-muted">You can request shelter by filling out a simple form. Our goal is to help individuals get back on their feet and achieve long-term stability. We believe that everyone deserves a safe place to call home, and we are here to help.</p>
                        </div>
                    </div>
                </div>
                <div class="col mb-4 requisition-type">
                    <div class="req-type">
                        <a href="#">
                            <img class="rounded img-fluid shadow w-100 fit-cover req-type-image" src="/img/products/Medical-Equipment-Discounts-for-Persons-with-Disabilities-e1600956443885.jpg" style="height: 250px;"></a>
                        <div class="py-4">
                            <span class="badge bg-primary mb-2">Secure</span>
                            <h4 class="fw-bold">Disability Service</h4>
                            <input type="hidden" value="3" />
                            <p class="text-muted">
                                You can request disability assistance by filling out a simple form.&nbsp;We are committed to providing compassionate, responsive care to all who request it, and we are here to help individuals with disabilities live their best lives.<br>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col mb-4 requisition-type">
                    <div class="req-type">
                        <a href="#">
                            <img class="rounded img-fluid shadow w-100 fit-cover req-type-image" src="/img/products/istockphoto-1217408094-612x612.jpg" style="height: 250px;"></a>
                        <div class="py-4">
                            <span class="badge bg-primary mb-2">Secure</span>
                            <h4 class="fw-bold">Animal Service</h4>
                            <input type="hidden" value="4" />
                            <p class="text-muted">You can request animal services by filling out a simple form.&nbsp;Whether you need routine veterinary care or emergency support, our team is here to help you and your pets live your best lives together</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <div class="modal fade" role="dialog" tabindex="-1" id="user-login-modal">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-dialog modal-login">
                    <div class="modal-content">
                        <div class="modal-header" style="padding: 0px;">
                            <h4 class="h4 modal-title">Requisition Form</h4>
                        </div>
                        <div class="modal-body">
                            <form id="user-login-form" method="post">
                                <asp:HiddenField runat="server" ID="hfRequestType" />
                                <div>
                                    <img class="rounded img-fluid shadow w-100 fit-cover" id="user-login-req-type" src="/img/products/depositphotos_98505198-stock-photo-fresh-vegetables-fruits-and-other.jpg"
                                        style="height: 160px; margin-bottom: 1rem; border-radius: 3px !important;">
                                </div>
                                <div class="form-group">
                                    <i class="far fa-clipboard fa-user"></i>
                                    <input class="form-control" type="text" id="user-login-req-placeholder" placeholder="Placeholder" required="required" readonly="readonly" style="user-select: none; pointer-events: none;">
                                </div>

                                <div class="form-group">
                                    <i class="fa fa-star fa-user"></i>
                                    <input class="form-control" type="email" id="txtemail" runat="server" required="required" name="email" placeholder="Email">
                                </div>
                                <div class="form-group">
                                    <i class="fa fa-star fa-user"></i>
                                    <input class="form-control" type="text" id="txtName" runat="server" required="required" name="name" placeholder="Name">
                                </div>
                                <div class="form-group">
                                    <i class="fa fa-star fa-user"></i>
                                    <input class="form-control" type="password" id="txtPassword" runat="server" required="required" name="Password" placeholder="Password">
                                </div>
                                <div class="form-group">
                                    <i class="fa fa-birthday-cake fa-lock"></i>
                                    <input class="form-control" runat="server" id="txtdob" type="date" name="dob">
                                </div>
                                <div class="d-flex justify-content-center form-group" id="user-login-submit" style="margin-top: 1REM;">
                                    <asp:Button class="btn btn-primary btn-block btn-lg" runat="server" ID="btnUserLogin" Text="Submit" OnClick="btnUserLogin_Click" />
                                    <%--<button class="btn btn-primary btn-block btn-lg"   type="submit" value="Login" onclick="btnUserLogin_Click" id=""  >Submit</button>--%>
                                </div>
                            </form>
                            <p class="text-center" style="font-size: 14px; margin-top: 1rem; color: rgb(135,135,135);">*Remember that your <strong>email </strong>and <strong>date of birth</strong> is used to authenticate you</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
