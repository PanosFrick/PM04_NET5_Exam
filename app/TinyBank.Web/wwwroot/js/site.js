// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.js-update-customer').on('click',
    (event) => {
        debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();
        let vatNumber = $('.js-vat-number').val();


        console.log(`${firstName} ${lastName} ${vatNumber}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName,
            vatNumber: vatNumber
        });

        // ajax call
        let result = $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            // success
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });


// #region exam

$('.js-checkout').on('click',
    () => {
        let cardNumber = $('.js-checkout-card-number').val();
        let expirationYear = $('.js-checkout-expiration-year').val();
        let expirationMonth = $('.js-checkout-expiration-month').val();
        let amount = $('.js-checkout-amount').val();

        let data = JSON.stringify({
            cardNumber: cardNumber,
            expirationYear: expirationYear,
            expirationMonth: expirationMonth,
            amount: amount
        });
        
        $('.formdiv').addClass('invisible');
        $('.spinner').removeClass('invisible');
        $('.alert-success').addClass('invisible');
        $('.alert-danger').addClass('invisible');
        

        
        let result = $.ajax({
            url: '/card/checkout',
            method: 'POST',
            contentType: 'application/json',
            data: data
        }).done(r => {
            if (r.code == 200) {
                $('.alert-success').removeClass('invisible');
            }
            if (r.code == 400 || r.code == 500) {
                $('.alert-danger').removeClass('invisible');
                $('.checkout-validation-errors').text(r.errorText);
                $('.formdiv').removeClass('invisible');
            }
        }).fail(f => {
            
        }).always(f => {
            $('.spinner').addClass('invisible');

        });
    }
);

// #endregion exam
