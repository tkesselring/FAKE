using System.Linq;
using Fake;
using Machine.Specifications;

namespace Test.FAKECore.Globbing.TestSample1
{
    public class when_enumerating_a_file_set : when_extracting_zip
    {
        static FileSystem.FileIncludes _files;
        Because of = () => _files = FileSystem.Include("temptest/SampleApp/bin/**/*.*");

        It should_find_ilmerge = () => _files.First().ShouldEndWith("ilmerge.exclude");
        It should_find_sample_app = () => _files.Skip(1).First().ShouldEndWith("SampleApp.dll");
        It should_match_2_files = () => _files.Count().ShouldEqual(2);
    }

    public class when_excluding_a_file : when_extracting_zip
    {
        Because of = () => Files =
            FileSystem
                .Include("temptest/SampleApp/bin/**/*.*")
                .ButNot("temptest/SampleApp/bin/**/ilmerge.exclude")
                .ToArray();

        It should_find_sample_app = () => Files.First().ShouldEndWith("SampleApp.dll");
        It should_match_1_file = () => Files.Count().ShouldEqual(1);
    }

    public class when_excluding_all_files : when_extracting_zip
    {
        Because of = () => Files =
            FileSystem
                .Include("temptest/SampleApp/bin/**/*.*")
                .ButNot("temptest/SampleApp/bin/**/*.*")
                .ToArray();

        It should_match_no_file = () => Files.Count().ShouldEqual(0);
    }

    public class when_scanning_for_any_dll_in_a_special_basefolder : when_extracting_zip
    {
        Because of = () => Files = FileSystem.Include("**/*.dll").SetBaseDirectory("temptest/").ToArray();

        It should_find_sample_app = () => Files.First().ShouldEndWith("SampleApp.dll");
        It should_match_1_file = () => Files.Length.ShouldEqual(1);
    }
}