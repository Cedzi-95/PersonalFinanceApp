public interface ImenuService
{
    void SetMenu(Menu menu);
    Menu GetMenu();
}

public class SimpleMenuService : ImenuService
{
    private Menu menu = new EmptyMenu();

    public Menu GetMenu()
    {
        return menu;
    }

    public void SetMenu(Menu menu)
    {
        this.menu = menu;
        this.menu.Display();
    }

}

class EmptyMenu : Menu
{
    public override void Display()
    {
        
    }
}