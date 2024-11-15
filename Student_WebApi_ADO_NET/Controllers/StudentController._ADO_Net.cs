//using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
//using ServicesContracts;
using System.Net;
using Student_WebApi_ADO_Net.Tools;
using Student_WebApi_ADO_NET.Tools;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student_WebApi_ADO_Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController_ADO_Net : ControllerBase
    {
        //private IRepositoryWrapper _repositoryWrapper;
        //private ILoggerManager _logger;

#if Use_Hub_Logic_On_ServerSide
        private readonly IHubContext<BroadcastHub> _broadcastHub;
#endif
        //public StudentController_ADO_Net(ILoggerManager logger,
        //                                 IRepositoryWrapper repositoryWrapper)
        //{
        //    this._logger = logger;
        //    this._repositoryWrapper = repositoryWrapper;
        //}

        [HttpGet("GetStudents_ADO_Net")]
        public async Task<IActionResult> GetStudents_ADO_Net(string UserName = "No Name",
                                                     bool IncludeRelations = true)
        {
            try
            {
                //IEnumerable<Student> StudentList = new List<Student>();

                //if (false == IncludeRelations)
                //{
                //    ToolsDataBaseLowerLayer.WatchStudentList(ToolsDataBaseLowerLayer.GetSQLCommandAndFields());
                //}
                //else
                //{
                //    ToolsDataBaseLowerLayer.WatchStudentList(ToolsDataBaseLowerLayer.GetSQLCommandAndFields());
                //}

                //StudentList = await this._repositoryWrapper.StudentRepositoryWrapper.FindAll();

                //List<StudentDto> StudentDtoList;

                //StudentDtoList = StudentList.Adapt<StudentDto[]>().ToList();

                //this._logger.LogInfo($"All Students have been read from GetStudents action by {UserName}");
                var StudentList = DatabaseInterface.ExecuteDatabaseReadCommand<Student>(DatabaseCommandStrings.SQLString_SP);
                return Ok(StudentList);
            }
            catch (Exception Error)
            {
                //this._logger.LogError($"Something went wrong inside GetStudents action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        //[HttpGet("GetStudent/{StudentID}")]
        //public async Task<IActionResult> GetStudent(int StudentID,
        //                                            string UserName = "No Name",
        //                                            bool UseLazyLoading = true)
        //{
        //    try
        //    {
        //        if (false == UseLazyLoading)
        //        {
        //            _repositoryWrapper.StudentRepositoryWrapper.DisableLazyLoading();
        //        }
        //        else
        //        {
        //            _repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();
        //        }
        //        //this._repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();

        //        Student Student_Object = await this._repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

        //        if (null == Student_Object)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            StudentDto StudentDto_Object = Student_Object.Adapt<StudentDto>();
        //            this._logger.LogInfo($"Student with StudentID {StudentID} has been read from GetStudent action by {UserName}");
        //            return Ok(StudentDto_Object);
        //        }
        //    }
        //    catch (Exception Error)
        //    {
        //        this._logger.LogError($"Something went wrong inside GetStudent action for {UserName} : {Error.Message}");
        //        return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
        //    }
        //}

        //[HttpGet("GetStudentsInTeam/{TeamID}")]
        //public async Task<IActionResult> GetStudentsInTeam(int TeamID,
        //                                                   string UserName = "No Name")
        //{
        //    try
        //    {
        //        IEnumerable<Student> StudentList = new List<Student>();

        //        this._repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();

        //        StudentList = await this._repositoryWrapper.StudentRepositoryWrapper.GetStudentsInTeam(TeamID);

        //        List<StudentDto> StudentDtoList;

        //        StudentDtoList = StudentList.Adapt<StudentDto[]>().ToList();

        //        this._logger.LogInfo($"All Students within TeamID : {TeamID} has been read from GetStudentsInTeam action by {UserName}");
        //        return Ok(StudentDtoList);
        //    }
        //    catch (Exception Error)
        //    {
        //        this._logger.LogError($"Something went wrong inside GetStudentsInTeam action for {UserName} : {Error.Message}");
        //        return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
        //    }
        //}

        // POST: api/Student
        [HttpPost("CreateStudent_ADO_Net")]
        public async Task<IActionResult> CreateStudent_ADO_Net([FromBody] StudentForSaveDto StudentForSaveDto_Object,
                                                                string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object = StudentForSaveDto_Object.Adapt<Student>();

                int SaveResult = Student_Object.Insert();

                if (SaveResult >= 0)
                {
                    return Ok($"Student : {Student_Object.StudentName} oprettet !!!");
                }
                else
                {
                    return BadRequest($"Noget gik galt, da {Student_Object.StudentName} : skulle oprettes !!!");
                }
            }
            catch (Exception Error)
            {
                //_logger.LogError($"Something went wrong inside CreateStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/Student/5
        [HttpPut("UpdateStudent_ADO_Net/{StudentID}")]
        public async Task<IActionResult> UpdateStudent_ADO_Net(int StudentID,
                                                               [FromBody] StudentForUpdateDto StudentForUpdateDto_Object,
                                                               string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsUpdated = 0;

                if (StudentID != StudentForUpdateDto_Object.StudentID)
                {
                    //_logger.LogError($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                    return BadRequest($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                }

                if (!ModelState.IsValid)
                {
                    //_logger.LogError($"ModelState is Invalid for {UserName} in action UpdateStudent");
                    return BadRequest(ModelState);
                }

                //Student Student_Object = await _repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

                //if (null == Student_Object)
                //{
                //    return NotFound();
                //}

                Student Student_Object = new Student();
                Student_Object = StudentForUpdateDto_Object.Adapt<Student>();

                int UpdateResult = Student_Object.Update();

                if (UpdateResult >= 0)
                {
                    return Ok($"Student med StudentId: {Student_Object.StudentID} opdateret !!!");
                }
                else
                {
                    return BadRequest($"Noget gik galt, da Student med StudentId: {Student_Object.StudentID} : skulle opdateres !!!");
                }

                //TypeAdapter.Adapt(StudentForUpdateDto_Object, Student_Object);

                //                await _repositoryWrapper.StudentRepositoryWrapper.Update(Student_Object);

                //                NumberOfObjectsUpdated = await _repositoryWrapper.Save();

                //                if (1 == NumberOfObjectsUpdated)
                //                {
                //#if Use_Hub_Logic_On_ServerSide
                //                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentDataMessage");
                //#endif
                //                    _logger.LogInfo($"Student with ID : {Student_Object.StudentID} has been updated by {UserName} !!!");
                //                    return Ok($"Student with ID : {Student_Object.StudentID} has been updated by {UserName} !!!"); ;
                //                }
                //                else
                //                {
                //                    _logger.LogError($"Error when updating Student with ID : {Student_Object.StudentID} by {UserName} !!!");
                //                    return BadRequest($"Error when updating Student with ID : {Student_Object.StudentID} by {UserName} !!!");
                //                }
            }
            catch (Exception Error)
            {
                //_logger.LogError($"Something went wrong inside UpdateStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        //        // DELETE: api/Student/5
        //        [HttpDelete("DeleteStudent/{StudentID}")]
        //        public async Task<IActionResult> DeleteStudent(int StudentID,
        //                                                       string UserName = "No Name")
        //        {
        //            try
        //            {
        //                int NumberOfObjectsDeleted;

        //                Student Student_Object = await this._repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

        //                if (null == Student_Object)
        //                {
        //                    this._logger.LogError($"Student with ID {StudentID} not found inside action DeleteStudent for {UserName}");
        //                    return NotFound();
        //                }

        //                await this._repositoryWrapper.StudentRepositoryWrapper.Delete(Student_Object);

        //                NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

        //                if (1 == NumberOfObjectsDeleted)
        //                {
        //#if Use_Hub_Logic_On_ServerSide
        //                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentDataMessage");
        //#endif
        //                    this._logger.LogInfo($"Student with ID {StudentID} has been deleted in action DeleteStudent by {UserName}");
        //                    return Ok($"Student with ID {StudentID} has been deleted in action DeleteStudent by {UserName}");
        //                }
        //                else
        //                {
        //                    _logger.LogError($"Error when deleting Student with ID : {StudentID} by {UserName} !!!");
        //                    return BadRequest($"Error when deleting Student with ID : {StudentID} by {UserName} !!!");
        //                }
        //            }
        //            catch (Exception Error)
        //            {
        //                _logger.LogError($"Something went wrong inside DeleteStudent action for {UserName}: {Error.Message}");
        //                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
        //            }
        //        }
    }
}
