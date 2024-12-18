public interface ImenuService
{
    void SetMenu(Menu menu);
    Menu GetMenu();
}

public class SimpleMenuService : ImenuService
{
    private Menu?  currentMenu;

    public Menu GetMenu()
    {
        if (currentMenu == null)
        {
            throw new ArgumentException("No current menu is set");
        }
        return currentMenu;
    }

    public void SetMenu(Menu menu)
    {
        currentMenu = menu;
        currentMenu.Display();
    }

}

class EmptyMenu : Menu
{
    public override void Display()
    {
        
    }
}