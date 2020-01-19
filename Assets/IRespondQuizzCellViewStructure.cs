/**
 * Interface that defines the method that will be called to show information/text in cell view
 * Info: A cell view is the cell that is  
 */
public interface IRespondQuizzCellViewStructure<T>
{
    T GetDataToShowInCellView();
    void SetDataToShowInCellView(T data);
}
