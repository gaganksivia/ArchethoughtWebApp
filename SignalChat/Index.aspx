<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Main.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SignalChat.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="bg-dark">
        <div class="container pt-4 pt-xl-5">
            <div class="row pt-5">
                <div class="col-md-8 col-xl-6 text-center text-md-start mx-auto">
                    <div class="text-center">
                        <p class="fw-bold text-success mb-2" style="font-size: 1.25rem;">We are listening to you</p>
                        <h1 class="fw-bold">A Safe Haven for Those in Need</h1>
                    </div>
                </div>
                <div class="col-12 col-lg-10 mx-auto">
                    <div class="position-relative" style="display: flex; flex-wrap: wrap; justify-content: flex-end;">
                        <div style="position: relative; flex: 0 0 45%; transform: translate3d(-15%, 35%, 0);">
                            <img class="img-fluid parallax-image" data-bss-parallax="" data-bss-parallax-speed="0.8" src="/img/products/istockphoto-1347286761-612x612.jpg" style="height: 100%; /*flex-shrink: 0; *//*min-width: 100%; *//*min-height: 100%; */object-fit: cover;"></div>
                        <div style="position: relative; flex: 0 0 45%; transform: translate3d(-5%, 10%, 0);">
                            <img class="img-fluid parallax-image" data-bss-parallax="" data-bss-parallax-speed="0.4" src="/img/products/help-image.png" width="349" height="419"></div>
                        <div style="position: relative; flex: 0 0 60%; transform: translate3d(0, 0%, 0);">
                            <img class="img-fluid parallax-image" data-bss-parallax="" data-bss-parallax-speed="0.25" src="/img/products/istockphoto-1330442385-612x612.jpg"></div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <section>
        <div class="container py-5">
            <div class="mx-auto" style="max-width: 900px;">
                <div class="row row-cols-1 row-cols-md-2 d-flex justify-content-center">
                    <div class="col mb-4">
                        <div class="card bg-primary-light">
                            <div class="card-body text-center px-4 py-5 px-md-5">
                                <p class="fw-bold text-primary card-text mb-2">Fully Secure</p>
                                <h5 class="fw-bold card-title mb-3">We are using the most secure ways of communication to provide a safe environment for people to reach out for help</h5>
                            </div>
                        </div>
                    </div>
                    <div class="col mb-4">
                        <div class="card bg-secondary-light">
                            <div class="card-body text-center px-4 py-5 px-md-5">
                                <p class="fw-bold text-secondary card-text mb-2">Fully Responsive</p>
                                <h5 class="fw-bold card-title mb-3">We try our best to connect you with someone that can provide you what you need</h5>
                            </div>
                        </div>
                    </div>
                    <div class="col mb-4">
                        <div class="card bg-info-light">
                            <div class="card-body text-center px-4 py-5 px-md-5">
                                <p class="fw-bold text-info card-text mb-2">Fully Managed</p>
                                <h5 class="fw-bold card-title mb-3">All the means of communication and requests are overseen by our team to make sure the best possible service is provided</h5>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <section style="padding-bottom: 200px;">
        <div class="container bg-dark py-5">
            <div class="row">
                <div class="col-md-8 col-xl-6 text-center mx-auto">
                    <p class="fw-bold text-success mb-2">Learn and Participate in Our Services</p>
                    <h3 class="fw-bold">Together, We Can Make a Difference</h3>
                </div>
            </div>
            <div class="py-5 p-lg-5">
                <div class="row row-cols-1 row-cols-md-2 mx-auto" style="max-width: 900px;">
                    <div class="col mb-5">
                        <div class="card shadow-sm">
                            <div class="card-body px-4 py-5 px-md-5">
                                <div class="bs-icon-lg d-flex justify-content-center align-items-center mb-3 bs-icon" style="top: 1rem; right: 1rem; position: absolute;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" fill="currentColor" viewBox="0 0 16 16" class="bi bi-shield-fill-plus text-success">
                                        <path fill-rule="evenodd" d="M8 0c-.69 0-1.843.265-2.928.56-1.11.3-2.229.655-2.887.87a1.54 1.54 0 0 0-1.044 1.262c-.596 4.477.787 7.795 2.465 9.99a11.777 11.777 0 0 0 2.517 2.453c.386.273.744.482 1.048.625.28.132.581.24.829.24s.548-.108.829-.24a7.159 7.159 0 0 0 1.048-.625 11.775 11.775 0 0 0 2.517-2.453c1.678-2.195 3.061-5.513 2.465-9.99a1.541 1.541 0 0 0-1.044-1.263 62.467 62.467 0 0 0-2.887-.87C9.843.266 8.69 0 8 0zm-.5 5a.5.5 0 0 1 1 0v1.5H10a.5.5 0 0 1 0 1H8.5V9a.5.5 0 0 1-1 0V7.5H6a.5.5 0 0 1 0-1h1.5V5z"></path>
                                    </svg>
                                </div>
                                <h5 class="fw-bold card-title">Give back!</h5>
                                <p class="text-muted card-text mb-4">Looking for a way to give back to your community? Consider supporting our organization, where we provide secure shelter and nutritious meals to those in need.</p>
                                <button class="btn btn-primary shadow" type="button">I Want to Help</button>
                            </div>
                        </div>
                    </div>
                    <div class="col mb-5">
                        <div class="card shadow-sm">
                            <div class="card-body px-4 py-5 px-md-5">
                                <div class="bs-icon-lg d-flex justify-content-center align-items-center mb-3 bs-icon" style="top: 1rem; right: 1rem; position: absolute;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" fill="currentColor" viewBox="0 0 16 16" class="bi bi-phone-landscape-fill text-success">
                                        <path d="M2 12.5a2 2 0 0 1-2-2v-6a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v6a2 2 0 0 1-2 2H2zm11-6a1 1 0 1 0 0 2 1 1 0 0 0 0-2z"></path>
                                    </svg>
                                </div>
                                <h5 class="fw-bold card-title">Are you in need?</h5>
                                <p class="text-muted card-text mb-4">Head to our Requisitions page, provide us with your need, some of your personal information and we will reach out to you as soon as possible.</p>
                                <button class="btn btn-primary shadow" type="button" onclick="window.open(&#39;requisition&#39;);">Requisition</button>
                            </div>
                        </div>
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
