import java.io.FileFilter;
import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

// TODO: fix login var

/**
 * Project ECCO - File manager class
 * 
 * @author Fernando M.A.d.S.
 *
 */
public class FileManager extends HttpServlet {

	private static final long serialVersionUID = 1L;
	private static String login = "feanndor"; // session var should come here
	private static String usersPath = System.getProperty("user.dir")+File.separator+"htdocs"+File.separator+"ecco"+File.separator+"users"+File.separator;
	private static File dir = new File(usersPath+login+File.separator);
	static boolean existDirectories = false;
	static int isDirectory = 0;

	public FileFilter filterFiles(File dir) {
		return (new FileFilter() {
			public boolean accept(File pathname) {
				return !(pathname.isDirectory());
			}
		});
	}

	public void listProjects(File dir, PrintWriter out) {
		try{
			File[] dirs = dir.listFiles();
			for (int i = 0; i < dirs.length; i++) {
				if (dirs[i].isDirectory()) {
					out.write("<project name="+dirs[i].getName()+">");
				} else {
					continue;
				}
				listDir(dirs[i],out);
				out.write("</project>");
			}
			File[] files = dir.listFiles(filterFiles(dir));
			for (int i = 0; i < files.length; i++) {
				System.out.println(files[i].getName());
			}
		}catch(Exception e){
			out.println("<info>error</info>");
			return;
		}
	}
}
