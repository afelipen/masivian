
public class Printer {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		final int numMaxPrimes= 1000;
		try {
			int[] numPrimes = PrimeNumbersManager.generate(numMaxPrimes);		

			PrimeNumbersWriter printPrimes = new PrimeNumbersWriter();					
			printPrimes.print(numPrimes, numMaxPrimes);			 			
		}
		catch (Exception ex)
		{
			System.out.print(ex.getMessage());
		}			
	}

}
