using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] private Transform target;
    [SerializeField] private GetCoin[] coins;
    [SerializeField] private int count = 0;

    private IStoreController storeController;

    private string crystal10 = "crystal10";

    private void Start()
    {
        InitIAP();
    }

    private void InitIAP()
    {
        //버튼 활성 비활성 설정
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(crystal10, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        if(product.definition.id == crystal10)
        {
            GameManager.instance.Crystal += count;
            for (int i = 0; i < count; i++)
            {
                coins[i].target = target;
                coins[i].startPos = transform;
                coins[i].gameObject.SetActive(true);
            }
        }

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productID)
    {
        //버튼 이벤트 연결
        storeController.InitiatePurchase(productID);
    }

    private void CheckNonCOnsumalbe(string id)
    {
        //구매 영수증 확인
        var product = storeController.products.WithID(id);

        if(product != null)
        {
            bool isCheck = product.hasReceipt;
        }
    }
}
