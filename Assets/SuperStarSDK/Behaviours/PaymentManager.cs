using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var payment = new creaditcard();
        payment.DoPayment();


        var paymentdebit = new debitcard();
        paymentdebit.DoPayment();
    }


}

public class paymentMethod
{

    public void DoPayment(IPaymentMode mode)
    {

        mode.DoPayment();
    }

}


public interface IPaymentMode
{

    void DoPayment();
}


public class creaditcard : IPaymentMode
{

    public void DoPayment()
    {

        Debug.Log("creaditcard payment");
    }
}

public class debitcard : IPaymentMode
{
    public void DoPayment()
    {
        Debug.Log("debitcard payment");

    }

}