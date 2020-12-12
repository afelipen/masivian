
public class PrimeNumbersWriter {
	
	private int rowsPage = 50;
	private int colunmPage = 4;		
	
	public  void print(int numPrimes[],int numMaxPrimes)
	{	 		  
		int pageNumber=1;
		int pageOffset=1;
		while (pageOffset <= numMaxPrimes) {
			printHeader(numMaxPrimes,pageNumber);			 					 
			printPage(pageOffset,numPrimes,numMaxPrimes);			 
			pageNumber++;
			System.out.println("\f");
			pageOffset += this.rowsPage * this.colunmPage;
		}	
	}
	
	private  void printHeader(int numMaxPrimes, int pageNumber){		
		System.out.print("The First " + Integer.toString(numMaxPrimes) + " Prime Numbers === Page " + Integer.toString(pageNumber) + "\n");
	}
	
	
	private void printPage(int pageOffset,int[] primes,int numMaxPrimes) {
		int rowOffset;				 
		for (rowOffset=pageOffset; rowOffset <= pageOffset+ this.rowsPage-1;	rowOffset++) {
			printRow(rowOffset,numMaxPrimes,primes);
		}
	}
	
	private void printRow(int rowOffset,int numMaxPrimes,int[] primes) {
		int column;		
		for (column = 0; column <=  this.colunmPage - 1; column++)
		{
			int accumulator = rowOffset + column * this.rowsPage;
			if (accumulator <= numMaxPrimes)
			{
				System.out.printf("%10d",primes[accumulator]);
			}	
		}
								
		System.out.println();		
	}
}