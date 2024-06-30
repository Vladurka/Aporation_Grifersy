public interface IShop 
{
    public void BuySkope(ScopesParametrs scope);
    public void BuyBase();
    public void BuyAkBullets(int price);
    public void BuyRpgBullets(int price);
    public void BuyGrenade(int price);
    public void BuyMine(int price);
    public void SetPanel(bool state);
    public void Open();
    public void Close();
}
