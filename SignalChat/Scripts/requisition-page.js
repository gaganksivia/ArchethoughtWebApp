const requisitions = document.querySelectorAll('.req-type');
$(document).ready(function () {
    $('#admin-list-modal').modal('show');
    // Add click event listener to each image
    requisitions.forEach(req => {
        
        req.addEventListener('click', () => {
            //Show the modal
            $('#user-login-modal').modal('show');
            //Get the image src value of clicked requisition type
            const imgLink= $(req).find('.req-type-image').attr('src');
            //Get the header for name of the requisition type (Food, Shelter, ...)
            const reqType = $(req).find('h4').text();
            const reqTypeId = $(req).find('input').val(); 
            $('#ContentPlaceHolder1_hfRequestType').val(reqType + '$' + reqTypeId);
            //Set the image and header inside the modal img and input placeholder
            $('#user-login-req-type').attr('src',imgLink);
            $('#user-login-req-placeholder').attr('placeholder',reqType);
            
            // Display modal
            modal.style.display = 'block';
            // Add click event listener to close modal
            modal.addEventListener('click', () => {
            modal.style.display = 'none';
            });
        });
    });



    $('#user-login-form').submit(()=>{
        alert('User login attempted successfully');
        //Login function happens here
        //Can get the "name" attr value to access all the input field values
        //Form ID is: #user-login-form



    })
  });
 

  //This function disables the future dates in "Date of Birth" selection input
$(function(){
    var dtToday = new Date();

    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();

    if(month < 10)
        month = '0' + month.toString();
    if(day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;    
    $('#dateOfBirth').attr('max', maxDate);
});

function gotochatbox(name) {
    $('#ContentPlaceHolder1_hfselectedAdmin').val(name);
}